// <copyright file="FormDetailsViewModel.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Model to represent the entry form details
    /// </summary>
    public class FormDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the name of the entry
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the entry
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the gender of the entry
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the image path of the uploaded image
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        ///  Gets or sets the image width property
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// Gets or sets the image height property
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Gets or sets the image size property
        /// </summary>
        public long ImageSize { get; set; }

        /// <summary>
        /// Gets or sets the image type property
        /// </summary>
        public string ImageType { get; set; }

        /// <summary>
        /// Method to serialize the form details using serialization. Not using this
        /// </summary>
        /// <param name="formdetails"> Form details</param>
        public void Serialize(FormDetailsViewModel formdetails)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FormDetailsViewModel));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, formdetails);
        }
    }
}