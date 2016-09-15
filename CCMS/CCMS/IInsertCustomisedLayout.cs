using System;
namespace ccms
{
    /// <summary>
    /// Interface defining replacement of custom tags.
    /// </summary>
    public interface IInsertCustomisedLayout
    {
        /// <summary>
        /// Interface method to map custom tags to custom output. Implementors needs to perform the following 
        /// basic logic om all tags to replace:
        /// <example>
        /// htmlSource = htmlSource.Replace("{CMS_CUSTOM_TAG}", myCustomMethod());
        /// </example>
        /// The custom method must return a string of the replaced html.
        /// </summary>
        /// <param name="htmlSource">The html source that contains the tags to be replaced.</param>
        /// <returns>The processed html after all custom tags have beem replaced.</returns>
        string get(string htmlSource);
    }
}
