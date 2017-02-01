﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Jaxx.FileSync.Shared.Interfaces;
using Jaxx.FileSync.GoogleDrive;

namespace Jaxx.FileSync
{
    public class GoogleDriveUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GoogleDriveUploader>().As<IUploader>().UsingConstructor(typeof(IEnumerable<string>), typeof(IGoogleAccountProvider)).InstancePerDependency();
            builder.RegisterType<GoogleCertServiceAccountProvider>().As<IGoogleAccountProvider>().UsingConstructor(typeof(string), typeof(string)).InstancePerLifetimeScope();            
        }
    }
}
