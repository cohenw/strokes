﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Strokes.Core;
using Strokes.Core.Service;
using Strokes.Core.Service.Model;
using System.Collections.Specialized;
using Strokes.Service.Data.Model;
using StructureMap;
using GalaSoft.MvvmLight.Messaging;
using Strokes.GUI.Views;
using System.Windows;
using System.IO;
using System.Globalization;
using Strokes.Service.Data;
using Strokes.GUI.Properties;

namespace Strokes.GUI
{
    public class AchievementPaneViewModel : ViewModelBase
    {
        private const string OrderedAchievementsFieldName = "AchievementsOrdered";
        private const string TotalAchievementsFieldName = "TotalAchievements";
        private const string TotalCompletedFieldName = "TotalCompleted";
        private const string PercentageCompletedFieldName = "PercentageCompleted";

        const string EnableText = "ENABLE STROKES FOR ALL PROJECTS";
        const string DisableText = "DISABLE STROKES FOR ALL PROJECTS";

        private readonly IAchievementService achievementService;
        private readonly ISettingsRepository settingsRepository;
        private readonly AchievementNotificationBox notificationBox; 

        public AchievementPaneViewModel()
        {
            AchievementsOrdered = new ObservableCollection<AchievementsPerCategory>();
            AvailableCultures = new ObservableCollection<CultureItem>();
            
            ResetCommand = new RelayCommand(ResetExecute);
            ToggleCommand = new RelayCommand(ToggleExecute);

            achievementService = ObjectFactory.TryGetInstance<IAchievementService>();
            settingsRepository = ObjectFactory.TryGetInstance<ISettingsRepository>();

            // Solves a design-time issue with StructureMap. 
            if (achievementService != null && settingsRepository != null)
            {
                notificationBox = new AchievementNotificationBox(achievementService);
                achievementService.AchievementsUnlocked += AchievementContext_AchievementsUnlocked;

                ReloadViewModel();
                LoadCultures();
            }
        }

        public ObservableCollection<AchievementsPerCategory> AchievementsOrdered
        {
            get;
            set;
        }

        public ObservableCollection<CultureItem> AvailableCultures
        {
            get;
            set;
        }

        public ICommand ResetCommand
        {
            get;
            private set;
        }

        public ICommand ToggleCommand
        {
            get;
            private set;
        }

        public CultureItem SelectedCulture
        {
            get;
            set;
        }

        public int TotalAchievements
        {
            get
            {
                return AchievementsOrdered.Sum(t => t.Count);
            }
        }

        public int TotalCompleted
        {
            get
            {
                return AchievementsOrdered.Sum(t => t.TotalCompleted);
            }
        }

        public int PercentageCompleted
        {
            get
            {
                return TotalAchievements != 0 ? (int)(((double)TotalCompleted / (double)TotalAchievements) * 100) : 0;
            }
        }

        public string Toggled
        {
            get;
            set;
        }

        private void OnSelectedCultureChanged()
        {
            var settings = settingsRepository.GetSettings();
            settings.PreferredLocale = SelectedCulture.CultureKey;
            settingsRepository.SaveSettings(settings);

            var test = CultureInfo.GetCultures(CultureTypes.UserCustomCulture);
            var test2 = CultureInfo.GetCultures(CultureTypes.ReplacementCultures);

            if (SelectedCulture.CultureKey == "en")
                Resources.Culture = CultureInfo.InvariantCulture;
            else                
                Resources.Culture = CultureInfo.GetCultureInfo(SelectedCulture.CultureKey);

            AppResources.Instance.Resources = new Resources();

            foreach (var achievement in AchievementsOrdered.SelectMany(x => x))
                achievementService.UpdateLocalization(achievement, Resources.Culture);

            ReloadViewModel();
        }

        private void LoadCultures()
        {
            var supportedCultures = new string[] { "en", "nl", "da", "ru" };
            
            foreach (var culture in supportedCultures)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);

                AvailableCultures.Add(new CultureItem
                {
                    DisplayName = cultureInfo.DisplayName.ToUpper(),
                    CultureKey = cultureInfo.Name
                });
            }

            var settings = settingsRepository.GetSettings();
            SelectedCulture = AvailableCultures.FirstOrDefault
            (
                x => x.CultureKey == settings.PreferredLocale
            );

            Toggled = settings.EnableInAllProjects ? DisableText : EnableText;
        }

        private void ResetExecute()
        {
            if (achievementService != null)
            {
                achievementService.ResetAchievementProgress();
                ReloadViewModel();
                Messenger.Default.Send(new ResetAchievementsMessage());
            }   
        }

        private void ToggleExecute()
        {
            var settings = settingsRepository.GetSettings();
            settings.EnableInAllProjects = !settings.EnableInAllProjects;
            settingsRepository.SaveSettings(settings);

            Toggled = settings.EnableInAllProjects ? DisableText : EnableText;
        }

        private void ReloadViewModel()
        {
            AchievementsOrdered.Clear();

            var achievements = Enumerable.Empty<Achievement>();
            if (achievementService != null)
                achievements = achievementService.GetAllAchievements();

            foreach (var category in achievements.AsCategories())
            {
                var sortedAchievements = category.Achievements
                        .Where(a => IsUnlocked(achievements, a))
                        .OrderByDescending(a => a.IsCompleted)
                        .ThenByDescending(a => a.DateCompleted)
                        .ThenBy(a => a.Name);

                if (sortedAchievements.Any())
                {
                    AchievementsOrdered.Add(new AchievementsPerCategory(sortedAchievements)
                    {
                        CategoryName = category.CategoryName,
                    });
                }
            }

            RaisePropertyChanged(OrderedAchievementsFieldName);
            RaisePropertyChanged(TotalCompletedFieldName);
            RaisePropertyChanged(PercentageCompletedFieldName);

            Messenger.Default.Send(new ResetAchievementsMessage());
        }

        private bool IsUnlocked(IEnumerable<Achievement> achievements, Achievement achievement)
        {
            if (achievement.DependsOn.Any() == false)
                return true;
            else if (achievement.DependsOn.All(a => a.IsCompleted == true && IsUnlocked(achievements, a)))
                return true;
            else
                return false;
        }

        private void AchievementContext_AchievementsUnlocked(object sender, AchievementEventArgs args)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => notificationBox.ShowAchievements(args.UnlockedAchievements)));

            foreach (var achievement in args.UnlockedAchievements)
            {
                var currentAchievement = achievement;
                var category = AchievementsOrdered.FirstOrDefault(c => c.CategoryName == currentAchievement.Category);
                if (category != null)
                    category.Update(currentAchievement);
            }

            RaisePropertyChanged(OrderedAchievementsFieldName);
            RaisePropertyChanged(TotalCompletedFieldName);
            RaisePropertyChanged(PercentageCompletedFieldName);
        }
    }

    public class CultureItem : IEquatable<CultureItem>
    {
        public string DisplayName
        {
            get;
            set;
        }

        public string CultureKey
        {
            get;
            set;
        }

        public bool Equals(CultureItem other)
        {
            if (other == null)
                return false;

            return this.CultureKey == other.CultureKey;
        }
    }

    public class AchievementsPerCategory : ObservableCollection<Achievement>, INotifyPropertyChanged
    {
        public AchievementsPerCategory()
        {
            CategoryName = "Unknown";
        }

        public AchievementsPerCategory(IEnumerable<Achievement> collection)
            : base(collection)
        {
            CategoryName = "Unknown";
        }

        public string CategoryName
        {
            get;
            set;
        }

        public int TotalCompleted
        {
            get
            {
                return this.Count(t => t.IsCompleted);
            }
        }

        public int PercentageCompleted
        {
            get
            {
                return this.Count != 0 ? (int)(((double)TotalCompleted / (double)this.Count) * 100) : 0;
            }
        }

        internal void Update(Achievement descriptor)
        {
            var achievementDescriptor = this.SingleOrDefault(a => a.Name == descriptor.Name);

            if (achievementDescriptor != null)
            {
                achievementDescriptor = descriptor;
                
                foreach (var unlocked in achievementDescriptor.Unlocks)
                    this.Add(unlocked);

                ApplySort();

                RaisePropertyChanged("TotalCompleted");
                RaisePropertyChanged("PercentageCompleted");
            }
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void ApplySort()
        {
             var sortedItems = this.OrderByDescending(a => a.IsCompleted)
                                   .ThenByDescending(a => a.DateCompleted)
                                   .ThenBy(a => a.Name)
                                   .ToList();

            foreach (var item in sortedItems)
            {
                Move(IndexOf(item), sortedItems.IndexOf(item));
            }
        }
    }
}
