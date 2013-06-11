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
using System.Linq;
using Telerik.OpenAccess;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Blogs;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Web.UI.PublicControls;

namespace Payscale.Sitefinity
{
    /// <summary>
    /// Custom Sitefinity Taxonomy Control to display taxonomy items for a specific Blog
    /// </summary>
    /// <remarks>Developed and tested on Sitefinity 5.2.3800.</remarks>
    public class BlogTaxonomyControl : TaxonomyControl
    {
        /* When adding the widget on the site, set the following properties as Toolbox item parameters:
            ContentType = Telerik.Sitefinity.Blogs.Model.BlogPost
            FieldName = Category
            TaxonomyId = e5cd6d69-1543-427b-ad62-688a99f5e7d4
         * We've only tested this with Category but in theory should be usable with Tags too.
        */

        /// <summary>
        /// Name of blog to filter on. This should be the same name you see in Blogs view.
        /// </summary>
        public string BlogName { get; set; }

        /// <summary>
        /// Returns a dictionary where the key is the taxon and value is
        /// the number is the count of the times that the taxon is used(marked)
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<ITaxon, uint> GetTaxaItemsCountForTaxonomy()
        {
            if (ShowItemCount)
            {
                var taxonomyIds = BlogsManager.GetManager(Provider)
                                              .GetBlogPosts()
                                              .Where(b => b.Status == ContentLifecycleStatus.Live && b.Parent.Title == BlogName && b.Visible)
                                              .SelectMany(b => b.GetValue<TrackedList<Guid>>(FieldName))
                                              .GroupBy(id => id);

                return taxonomyIds.ToDictionary(
                    group => CurrentTaxonomyManager.GetTaxon(group.Key),
                    group => (uint)group.Count());
            }
            else
            {
                var taxonomyIds = BlogsManager.GetManager(Provider)
                    .GetBlogPosts()
                    .Where(b => b.Status == ContentLifecycleStatus.Live && b.Parent.Title == BlogName && b.Visible)
                    .SelectMany(b => b.GetValue<TrackedList<Guid>>(FieldName))
                    .Distinct(); 

                return taxonomyIds.ToDictionary(
                    id => CurrentTaxonomyManager.GetTaxon(id),
                    id => (uint) 1); // ShowItemCount is false so this doesn't matter.
            }
        }
    }
}
