#pragma checksum "C:\Users\pc\source\repos\firstProjecthallBooking\Views\Home\AboutUs.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "90c65286cbaeb0b5038c771ffcde9c94133205a1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_AboutUs), @"mvc.1.0.view", @"/Views/Home/AboutUs.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\_ViewImports.cshtml"
using FirstProjectHallBooking;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\_ViewImports.cshtml"
using FirstProjectHallBooking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\_ViewImports.cshtml"
using FirstProjectHallBooking.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90c65286cbaeb0b5038c771ffcde9c94133205a1", @"/Views/Home/AboutUs.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88cbf7f8f79040cfc85347f0d308bfefc269d4f9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_AboutUs : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HomeVM>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\Home\AboutUs.cshtml"
   
    Layout = "~/views/Shared/_HomeLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- bradcam_area_start -->\r\n    <div class=\"bradcam_area \"");
            BeginWriteAttribute("style", " style=\"", 137, "\"", 210, 3);
            WriteAttributeValue("", 145, "background-image:url(", 145, 21, true);
#nullable restore
#line 6 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\Home\AboutUs.cshtml"
WriteAttributeValue("", 166, Url.Content("~/Image/"+@Model.home.Image1), 166, 43, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 209, ")", 209, 1, true);
            EndWriteAttribute();
            WriteLiteral(@">
        <h3>About Hall Booking </h3>
    </div>
    <!-- bradcam_area_end -->

    <!-- about_area_start -->
    <div class=""about_area"">
        <div class=""container"">
            <div class=""row"">
                <div class=""col-xl-5 col-lg-5"">
                    <div class=""about_info"">
                        <div class=""section_title mb-20px"">
                            <span>About Us</span>
                            <h3>Description of Hall Booking System</h3>
                        </div>
                        <p>");
#nullable restore
#line 21 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\Home\AboutUs.cshtml"
                      Write(Model.Aboutu.Descriptoin);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                       
                    </div>
                </div>
                <div class=""col-xl-7 col-lg-7"">
                    <div class=""about_thumb d-flex"">
                        <div class=""img_1"">
                            <img");
            BeginWriteAttribute("src", " src=", 1049, "", 1097, 1);
#nullable restore
#line 28 "C:\Users\pc\source\repos\firstProjecthallBooking\Views\Home\AboutUs.cshtml"
WriteAttributeValue("", 1054, Url.Content("~/Image/"+@Model.home.Image3), 1054, 43, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 1097, "\"", 1103, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                        </div>
                    
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- about_area_end -->

    <!-- about_info_area_start -->
   
    <!-- about_info_area_start -->

    <!-- about_main_info_start -->
");
            WriteLiteral("    <!-- about_main_info_end -->\r\n\r\n    <!-- forQuery_start -->\r\n\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HomeVM> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591