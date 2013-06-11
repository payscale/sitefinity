#region Apache license
/*Copyright 2013 PayScale, Inc.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Blogs.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Modules.Blogs;
using Telerik.Sitefinity.Modules.GenericContent.Archive;
using Telerik.Sitefinity.Web.UI.PublicControls;
using Telerik.Sitefinity.Web.UrlEvaluation;

namespace Payscale.Sitefinity
{
    /// <summary>
    /// Custom Sitefinity Control to display Archive links for a specific Blog
    /// </summary>
    /// <remarks>Developed and tested on Sitefinity 5.2.3800.</remarks>
    public class BlogArchiveControl : ArchiveControl
    {
        /* When adding the widget on the site, set the following as a Toolbox item parameter:
            ContentType = Telerik.Sitefinity.Blogs.Model.BlogPost   
        */

        /// <summary>
        /// Name of blog to filter on. This should be the same name you see in Blogs view.
        /// </summary>
        public string BlogName { get; set; }

        public override void ConstructArchiveItems()
        {
            if (DateBuildOptions != DateBuildOptions.YearMonth)
            {
                // if we need to support Year or YearMonthDay in the future we can add a switch statement below and remove this check.
                throw new Exception("DateBuildOptions unsupported, must be YearMonth");
            }
            
            var blogs = BlogsManager.GetManager(Provider).GetBlogPosts();
            var items = (from b in blogs
                          where b.Status == ContentLifecycleStatus.Live && b.Parent.Title == BlogName && b.Visible
                          group b by new {y = b.PublicationDate.Year, m = b.PublicationDate.Month}
                          into grp
                          select new ArchiveItem(new DateTime(grp.Key.y, grp.Key.m, 1), grp.Count()))
                .OrderByDescending(ai => ai.Date)
                .ToList();

            DataBindArchiveRepeater(items);
        }
    }
}
