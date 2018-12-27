﻿using RemindXamarin.Models;
using RemindXamarin.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemindXamarin.Views
{
   
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewCategory : ContentPage
    {
        public string title = "Ajouter une catégorie";
        public Category category { get; set; }
        public ArrayList Icones { get; set; }

        public NewCategory ()
		{
 			InitializeComponent ();
                       
            Icones = new ArrayList() {
                "ic_access_alarm_black_36dp.png",
                "ic_access_time_black_36dp.png",
                "ic_add_a_photo_black_36dp.png",
                "ic_add_black_36dp.png",
                "ic_alarm_black_36dp.png",
                "ic_brush_black_36dp.png",
                "ic_build_black_36dp.png",
                "ic_cake_black_36dp.png",
                "ic_card_giftcard_black_36dp.png",
                "ic_check_circle_black_36dp.png",
                "ic_delete_black_36dp.png",
                "ic_desktop_windows_black_36dp.png",
                "ic_directions_black_36dp.png",
                "ic_directions_run_black_36dp.png",
                "ic_favorite_black_36dp.png",
                "ic_flight_takeoff_black_36dp.png",
                "ic_grade_black_36dp.png",
                "ic_location_on_black_36dp.png",
                "ic_message_black_36dp.png",
                "ic_movie_black_36dp.png",
                "ic_music_video_black_36dp.png",
                "ic_spa_black_36dp.png",
                "ic_thumb_up_black_36dp.png",
                "ic_voicemail_black_36dp.png",
            };

            category = new Category("now de la catégori", (String) Icones[0] , Color.FromHex("FF6A00"));

            BindingContext = this;

            pickerIcone.SelectedIndex = 0;
        }

        public void OnIconChanged()
        {
            category.icon = (String) Icones[pickerIcone.SelectedIndex];
        }

        public async Task Save_Clicked()
        {
            if(pickerIcone.SelectedIndex != -1)
            {
                if (!category.name.Equals(""))
                {
                    if (editedColor.Color != null)
                    {
                        category.color = editedColor.Color;
                        Tasker.Instance.addCategory(category);

                        MessagingCenter.Send(this, "AddCategory", category);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        DependencyService.Get<IMessageToast>().Show("selectionner une couleur !!!");
                    }
                }
                else
                {
                    DependencyService.Get<IMessageToast>().Show("selectionner un titre !!!");
                }
            }
            else
            {
                DependencyService.Get<IMessageToast>().Show("selectionner une icône !!!");
            }
        }


    }
}