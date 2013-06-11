sitefinity
==========

Telerik Sitefinity add-ons developed at PayScale

/Controls/BlogCategoriesControl.cs:
/Controls/BlogArchiveControl.cs:
The built-in controls for showing Archive or Categories links show you links based on all blog posts on your site (or at least in the provider).
We have two blogs (http://www.payscale.com/career-news and http://www.payscale.com/compensation-today/) served from the same custom provider, so
when we added an ArchiveControl to the career-news page, it was counting blog posts from the compensation-today blog.  There wasn't a way to 
filter it in the standard control.  The solution in both cases was the same - inherit from the standard control, add a property with blog
name, and override a single method.  The methods we overrode were not documented that we could find, so it took a bit of research, looking at
sample code, and experimentation.

