﻿using Google.Apis.Drive.v3;
using Jaxx.FileSync.Shared.Interfaces;
using System;
using System.Globalization;

namespace Jaxx.FileSync.GoogleDrive
{
    public class GoogleDriverDeleteController : IDeleteController
    {
        DriveService _service;

        public GoogleDriverDeleteController(IGoogleAccountProvider accountProvider)
        {            
            _service = accountProvider.CreateDriveService();
        }

        public bool DeleteAgedFiles(int fileAgeInDays, string folder, bool preview)
        {

            var folders = DriveApi.GetFilesByName(_service, folder, DriveApi.NameSearchOperators.Is);

            if (folders.Count != 1)
            {
                throw new ArgumentException($"{folder} not found or not unique in your store.", folder);
            }

            var time = DateTime.Now - new TimeSpan(fileAgeInDays, 0, 0, 0, 0);
            
            string timeString = time.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            var files = DriveApi.GetFiles(_service, $"modifiedTime <= '{timeString}' and '{folders[0].Id}' in parents");
            //var files = DriveApi.GetFiles(_service, $"modifiedTime <= '{timeString}'");

            foreach (var file in files)
            {
                if (preview)
                {
                    Console.WriteLine($"This would delete {file.Name}, {file.ModifiedTime} (preview mode).");
                }
                else DeleteObject(file.Id);
            }

            return true;
        }

        public bool DeleteObject(string objectId)
        {
            return DriveApi.DeleteObject(_service, objectId);
        }
    }
}
