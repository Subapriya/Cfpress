// <copyright file="CompetitionsViewModel.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;
    
    /// <summary>
    ///  Model to represent the various competitions 
    /// </summary>
    public class CompetitionsViewModel
    {
        /// <summary>
        ///  Gets or sets the name of the competition
        /// </summary>
        public string CompetitionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the newspaper in which the competition  is held
        /// </summary>
        public string NewspaperName { get; set; }

        /// <summary>
        /// Gets or sets the details of the competition entry form
        /// </summary>
        public FormDetailsViewModel FormDetails { get; set; }

        /// <summary>
        /// Gets or sets the xml string of the form details
        /// </summary>
        public string Formdetailsxml { get; set; }

        /// <summary>
        /// Gets or sets the earned votes
        /// </summary>
        public int EarnedVotes { get; set; }

        /// <summary>
        /// Method to convert the form details in to xml using linked queries
        /// </summary>
        /// <param name="formdetails">Form details instance</param>
        /// <returns>the serialized element</returns>
        public XElement SerializeXml(CompetitionsViewModel competitionmodel)
        {
            XElement serializedxml;
            try
            {
                serializedxml = new XElement(
                    "formfields",
                    new XAttribute("name", competitionmodel.FormDetails.Name),
                    new XAttribute("age", competitionmodel.FormDetails.Age),
                    new XAttribute("gender", !string.IsNullOrEmpty(competitionmodel.FormDetails.Gender) ? competitionmodel.FormDetails.Gender : string.Empty),
                    new XAttribute("imagepath", competitionmodel.FormDetails.ImagePath));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return serializedxml;
        }
    }
}