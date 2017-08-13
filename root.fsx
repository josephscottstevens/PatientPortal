#r "./packages/Suave/lib/net40/Suave.dll"
#load "pages/suaveHtml.fsx"
#load "pages/carePlan.fsx"
#load "pages/homePage.fsx"
#load "pages/feedback.fsx"
#load "pages/medications.fsx"
#load "pages/forms.fsx"
#load "pages/education.fsx"
#load "pages/proxies.fsx"
#load "pages/logout.fsx"

open SuaveHtml
open HomePage
open CarePlan
open Feedback
open Medications
open Forms
open Education
open Proxies
open Logout
let ul = tag "ul"
let li = tag "li"
let homeSvg = rawText """<svg class="homeSvg" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="512px" height="512px" viewBox="0 0 512 512" style="enable-background:new 0 0 512 512;" xml:space="preserve"> <g>	<path d="M512,296l-96-96V56h-64v80l-96-96L0,296v16h64v160h160v-96h64v96h160V312h64V296z"></path> </g> <g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let carePlanSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 60 60" style="enable-background:new 0 0 60 60;" xml:space="preserve"><g>	<path d="M38.914,0H6.5v60h47V14.586L38.914,0z M39.5,3.414L50.086,14H39.5V3.414z M8.5,58V2h29v14h14v42H8.5z"/>	<path d="M42.5,21h-16c-0.552,0-1,0.447-1,1s0.448,1,1,1h16c0.552,0,1-0.447,1-1S43.052,21,42.5,21z"/>	<path d="M22.875,18.219l-4.301,3.441l-1.367-1.367c-0.391-0.391-1.023-0.391-1.414,0s-0.391,1.023,0,1.414l2,2		C17.987,23.901,18.243,24,18.5,24c0.22,0,0.441-0.072,0.624-0.219l5-4c0.432-0.346,0.501-0.975,0.156-1.406		C23.936,17.943,23.306,17.874,22.875,18.219z"/>	<path d="M42.5,32h-16c-0.552,0-1,0.447-1,1s0.448,1,1,1h16c0.552,0,1-0.447,1-1S43.052,32,42.5,32z"/>	<path d="M22.875,29.219l-4.301,3.441l-1.367-1.367c-0.391-0.391-1.023-0.391-1.414,0s-0.391,1.023,0,1.414l2,2		C17.987,34.901,18.243,35,18.5,35c0.22,0,0.441-0.072,0.624-0.219l5-4c0.432-0.346,0.501-0.975,0.156-1.406		C23.936,28.943,23.306,28.874,22.875,29.219z"/>	<path d="M42.5,43h-16c-0.552,0-1,0.447-1,1s0.448,1,1,1h16c0.552,0,1-0.447,1-1S43.052,43,42.5,43z"/>	<path d="M22.875,40.219l-4.301,3.441l-1.367-1.367c-0.391-0.391-1.023-0.391-1.414,0s-0.391,1.023,0,1.414l2,2		C17.987,45.901,18.243,46,18.5,46c0.22,0,0.441-0.072,0.624-0.219l5-4c0.432-0.346,0.501-0.975,0.156-1.406		C23.936,39.943,23.306,39.874,22.875,40.219z"/></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let feedbackSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 31.012 31.012" style="enable-background:new 0 0 31.012 31.012;" xml:space="preserve"><g>	<g>		<path d="M25.109,21.51c-0.123,0-0.246-0.045-0.342-0.136l-5.754-5.398c-0.201-0.188-0.211-0.505-0.022-0.706			c0.189-0.203,0.504-0.212,0.707-0.022l5.754,5.398c0.201,0.188,0.211,0.505,0.022,0.706C25.375,21.457,25.243,21.51,25.109,21.51z			"/>		<path d="M5.902,21.51c-0.133,0-0.266-0.053-0.365-0.158c-0.189-0.201-0.179-0.518,0.022-0.706l5.756-5.398			c0.202-0.188,0.519-0.18,0.707,0.022c0.189,0.201,0.179,0.518-0.022,0.706l-5.756,5.398C6.148,21.465,6.025,21.51,5.902,21.51z"/>	</g>	<path d="M28.512,26.529H2.5c-1.378,0-2.5-1.121-2.5-2.5V6.982c0-1.379,1.122-2.5,2.5-2.5h26.012c1.378,0,2.5,1.121,2.5,2.5v17.047		C31.012,25.408,29.89,26.529,28.512,26.529z M2.5,5.482c-0.827,0-1.5,0.673-1.5,1.5v17.047c0,0.827,0.673,1.5,1.5,1.5h26.012		c0.827,0,1.5-0.673,1.5-1.5V6.982c0-0.827-0.673-1.5-1.5-1.5H2.5z"/>	<path d="M15.506,18.018c-0.665,0-1.33-0.221-1.836-0.662L0.83,6.155C0.622,5.974,0.6,5.658,0.781,5.449		c0.183-0.208,0.498-0.227,0.706-0.048l12.84,11.2c0.639,0.557,1.719,0.557,2.357,0L29.508,5.419		c0.207-0.181,0.522-0.161,0.706,0.048c0.181,0.209,0.16,0.524-0.048,0.706L17.342,17.355		C16.835,17.797,16.171,18.018,15.506,18.018z"/></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let medicationsSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.1.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 84.112 84.112" style="enable-background:new 0 0 84.112 84.112;" xml:space="preserve"><path d="M68.449,48.757H41.99l-3.873,3.873h7.409v23.581H27.479c-5.495,0-10.11-3.783-11.412-8.878	c-0.14,0.003-0.28,0.01-0.421,0.01c-1.212,0-2.423-0.118-3.617-0.351c1.231,7.417,7.688,13.092,15.449,13.092h40.97	c8.637,0,15.664-7.026,15.664-15.663C84.112,55.784,77.086,48.757,68.449,48.757z M64.88,76.014H51.911	c-0.917,0-1.66-0.743-1.66-1.66c0-0.917,0.743-1.66,1.66-1.66H64.88c0.917,0,1.66,0.743,1.66,1.66	C66.54,75.271,65.797,76.014,64.88,76.014z M71.497,76.014h-0.644c-0.917,0-1.66-0.743-1.66-1.66c0-0.917,0.743-1.66,1.66-1.66	h0.644c0.917,0,1.66,0.743,1.66,1.66C73.157,75.271,72.414,76.014,71.497,76.014z M55.692,8.6	c-2.947-2.948-6.881-4.571-11.076-4.571c-4.195,0-8.128,1.623-11.076,4.571l-28.97,28.97C1.623,40.517,0,44.45,0,48.645	c0,4.195,1.623,8.128,4.571,11.076c3.054,3.054,7.064,4.58,11.076,4.58c4.011,0,8.023-1.527,11.076-4.58l28.97-28.97	c2.948-2.948,4.571-6.881,4.571-11.076C60.264,15.48,58.64,11.547,55.692,8.6z M23.984,56.982c-4.598,4.597-12.078,4.597-16.674,0	c-2.216-2.216-3.437-5.177-3.437-8.337c0-3.16,1.221-6.121,3.437-8.337l12.761-12.761l16.674,16.674L23.984,56.982z M50.291,30.396	l-9.17,9.17c-0.324,0.324-0.749,0.486-1.174,0.486c-0.425,0-0.85-0.162-1.174-0.486c-0.648-0.648-0.648-1.699,0-2.347l9.171-9.17	c0.648-0.648,1.699-0.648,2.347,0C50.939,28.697,50.939,29.748,50.291,30.396z M54.97,25.717l-0.455,0.455	c-0.324,0.324-0.749,0.486-1.174,0.486c-0.425,0-0.849-0.162-1.174-0.486c-0.648-0.648-0.648-1.699,0-2.347l0.455-0.455	c0.648-0.648,1.699-0.648,2.347,0C55.618,24.018,55.618,25.069,54.97,25.717z"/><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let formsSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 512 512" style="enable-background:new 0 0 512 512;" xml:space="preserve"><g>	<g>		<path d="M451.457,138.456L317.892,4.891C314.721,1.719,310.427,0,306.087,0H72.348c-9.22,0-16.696,7.475-16.696,16.696v478.609			c0,9.22,7.475,16.696,16.696,16.696h367.304c9.22,0,16.696-7.475,16.696-16.696V150.261			C456.348,145.989,454.644,141.641,451.457,138.456z M322.783,57.002c8.112,8.112,68.448,68.448,76.563,76.563h-76.563V57.002z			 M422.957,478.609H89.043V33.391h200.348v116.87c0,9.22,7.475,16.696,16.696,16.696h116.87V478.609z"/>	</g></g><g>	<g>		<path d="M372.87,244.87H205.913c-9.22,0-16.696,7.475-16.696,16.696c0,9.22,7.475,16.696,16.696,16.696H372.87			c9.22,0,16.696-7.475,16.696-16.696C389.565,252.345,382.09,244.87,372.87,244.87z"/>	</g></g><g>	<g>		<path d="M372.87,311.652H205.913c-9.22,0-16.696,7.475-16.696,16.696s7.475,16.696,16.696,16.696H372.87			c9.22,0,16.696-7.475,16.696-16.696S382.09,311.652,372.87,311.652z"/>	</g></g><g>	<g>		<path d="M372.87,378.435H205.913c-9.22,0-16.696,7.475-16.696,16.696s7.475,16.696,16.696,16.696H372.87			c9.22,0,16.696-7.475,16.696-16.696S382.09,378.435,372.87,378.435z"/>	</g></g><g>	<g>		<circle cx="139.13" cy="261.565" r="16.696"/>	</g></g><g>	<g>		<circle cx="139.13" cy="328.348" r="16.696"/>	</g></g><g>	<g>		<circle cx="139.13" cy="395.13" r="16.696"/>	</g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let educationSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.1.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 485.5 485.5" style="enable-background:new 0 0 485.5 485.5;" xml:space="preserve"><g>	<g>		<path d="M422.1,126.2H126.4c-27.4,0-49.8-22.3-49.8-49.8c0-27.4,22.3-49.8,49.8-49.8h295.8c7.4,0,13.3-6,13.3-13.3			c0-7.4-6-13.3-13.3-13.3H126.4C84.3,0,50,34.3,50,76.4v332.7c0,42.1,34.3,76.4,76.4,76.4h295.8c7.4,0,13.3-6,13.3-13.3V139.5			C435.4,132.2,429.5,126.2,422.1,126.2z M408.8,458.9H126.4c-27.4,0-49.8-22.3-49.8-49.8V134.4c13.4,11.5,30.8,18.5,49.8,18.5			h282.4L408.8,458.9L408.8,458.9z"/>		<path d="M130.6,64.3c-7.4,0-13.3,6-13.3,13.3s6,13.3,13.3,13.3h249.8c7.4,0,13.3-6,13.3-13.3s-6-13.3-13.3-13.3H130.6z"/>		<path d="M177.4,400.7c1.5,0.5,3,0.8,4.5,0.8c5.5,0,10.6-3.4,12.5-8.8l16.2-45.3H273c0.5,0,1.1,0,1.6-0.1l16.2,45.4			c1.9,5.4,7.1,8.8,12.5,8.8c1.5,0,3-0.3,4.5-0.8c6.9-2.5,10.5-10.1,8-17l-60.6-169.9l0,0c-0.1-0.4-0.3-0.8-0.5-1.2			c-0.1-0.2-0.2-0.4-0.3-0.6c-0.1-0.2-0.2-0.4-0.3-0.6c-0.1-0.2-0.3-0.4-0.4-0.7c-0.1-0.1-0.2-0.3-0.3-0.4c-0.1-0.2-0.3-0.4-0.5-0.6			c-0.1-0.1-0.2-0.3-0.4-0.4c-0.1-0.2-0.3-0.3-0.5-0.5s-0.3-0.3-0.5-0.5c-0.1-0.1-0.3-0.2-0.4-0.4c-0.2-0.2-0.4-0.3-0.6-0.5			c-0.1-0.1-0.3-0.2-0.4-0.3c-0.2-0.1-0.4-0.3-0.6-0.4c-0.2-0.1-0.4-0.2-0.6-0.3s-0.4-0.2-0.6-0.3c-0.4-0.2-0.8-0.4-1.2-0.5l0,0H247			c-0.4-0.1-0.8-0.2-1.2-0.3c-0.2,0-0.3-0.1-0.5-0.1c-0.3-0.1-0.5-0.1-0.8-0.2c-0.2,0-0.4,0-0.6-0.1c-0.2,0-0.5-0.1-0.7-0.1			s-0.4,0-0.6,0c-0.2,0-0.4,0-0.7,0c-0.2,0-0.5,0-0.7,0.1c-0.2,0-0.4,0-0.6,0.1c-0.3,0-0.5,0.1-0.8,0.2c-0.2,0-0.3,0.1-0.5,0.1			c-0.4,0.1-0.8,0.2-1.1,0.3h-0.1l0,0c-0.4,0.1-0.8,0.3-1.2,0.5c-0.2,0.1-0.4,0.2-0.6,0.3c-0.2,0.1-0.4,0.2-0.6,0.3			c-0.2,0.1-0.4,0.3-0.7,0.4c-0.1,0.1-0.3,0.2-0.4,0.3c-0.2,0.2-0.4,0.3-0.6,0.5c-0.1,0.1-0.3,0.2-0.4,0.4c-0.2,0.2-0.3,0.3-0.5,0.5			s-0.3,0.3-0.5,0.5c-0.1,0.1-0.2,0.3-0.4,0.4c-0.2,0.2-0.3,0.4-0.5,0.6c-0.1,0.1-0.2,0.3-0.3,0.4c-0.1,0.2-0.3,0.4-0.4,0.6			c-0.1,0.2-0.2,0.4-0.3,0.6c-0.1,0.2-0.2,0.4-0.3,0.6c-0.2,0.4-0.4,0.8-0.5,1.2l0,0l-60.8,169.9			C166.9,390.6,170.5,398.2,177.4,400.7z M242.7,257.8l22.5,63h-45.1L242.7,257.8z"/>	</g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head
let proxiesSvg = rawText """<?xml version="1.0" encoding="iso-8859-1"?><!-- Generator: Adobe Illustrator 19.1.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  --><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 482.9 482.9" style="enable-background:new 0 0 482.9 482.9;" xml:space="preserve"><g>	<g>		<path d="M239.7,260.2c0.5,0,1,0,1.6,0c0.2,0,0.4,0,0.6,0c0.3,0,0.7,0,1,0c29.3-0.5,53-10.8,70.5-30.5			c38.5-43.4,32.1-117.8,31.4-124.9c-2.5-53.3-27.7-78.8-48.5-90.7C280.8,5.2,262.7,0.4,242.5,0h-0.7c-0.1,0-0.3,0-0.4,0h-0.6			c-11.1,0-32.9,1.8-53.8,13.7c-21,11.9-46.6,37.4-49.1,91.1c-0.7,7.1-7.1,81.5,31.4,124.9C186.7,249.4,210.4,259.7,239.7,260.2z			 M164.6,107.3c0-0.3,0.1-0.6,0.1-0.8c3.3-71.7,54.2-79.4,76-79.4h0.4c0.2,0,0.5,0,0.8,0c27,0.6,72.9,11.6,76,79.4			c0,0.3,0,0.6,0.1,0.8c0.1,0.7,7.1,68.7-24.7,104.5c-12.6,14.2-29.4,21.2-51.5,21.4c-0.2,0-0.3,0-0.5,0l0,0c-0.2,0-0.3,0-0.5,0			c-22-0.2-38.9-7.2-51.4-21.4C157.7,176.2,164.5,107.9,164.6,107.3z"/>		<path d="M446.8,383.6c0-0.1,0-0.2,0-0.3c0-0.8-0.1-1.6-0.1-2.5c-0.6-19.8-1.9-66.1-45.3-80.9c-0.3-0.1-0.7-0.2-1-0.3			c-45.1-11.5-82.6-37.5-83-37.8c-6.1-4.3-14.5-2.8-18.8,3.3c-4.3,6.1-2.8,14.5,3.3,18.8c1.7,1.2,41.5,28.9,91.3,41.7			c23.3,8.3,25.9,33.2,26.6,56c0,0.9,0,1.7,0.1,2.5c0.1,9-0.5,22.9-2.1,30.9c-16.2,9.2-79.7,41-176.3,41			c-96.2,0-160.1-31.9-176.4-41.1c-1.6-8-2.3-21.9-2.1-30.9c0-0.8,0.1-1.6,0.1-2.5c0.7-22.8,3.3-47.7,26.6-56			c49.8-12.8,89.6-40.6,91.3-41.7c6.1-4.3,7.6-12.7,3.3-18.8c-4.3-6.1-12.7-7.6-18.8-3.3c-0.4,0.3-37.7,26.3-83,37.8			c-0.4,0.1-0.7,0.2-1,0.3c-43.4,14.9-44.7,61.2-45.3,80.9c0,0.9,0,1.7-0.1,2.5c0,0.1,0,0.2,0,0.3c-0.1,5.2-0.2,31.9,5.1,45.3			c1,2.6,2.8,4.8,5.2,6.3c3,2,74.9,47.8,195.2,47.8s192.2-45.9,195.2-47.8c2.3-1.5,4.2-3.7,5.2-6.3			C447,415.5,446.9,388.8,446.8,383.6z"/>	</g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head

type Page =
  {
    Url: string
    Icon: Node 
    Label: Node
  }
let nav = tag "nav"
let isProxy = false

let patientDisplayTopRight =
  let cls = ["style", "display: inline-block; margin: 5px;"]
  let ifProxyLst = 
    [
      p cls (text "Patient")
      p cls (text "Sarah O'Conner")
    ]

  let lst =
    [
      p cls (text "Welcome")
      p cls (text "Keven Homer")
    ]
  let lst =
    if isProxy then 
      lst 
    else 
      lst @ ifProxyLst
  div ["style", "display: block; margin: 5px"] lst

let pages = 
  [
    { Url ="/"; Icon = homeSvg; Label = div [] (text "Welcome") }
    { Url ="/careplan"; Icon = carePlanSvg; Label = div [] (text "Care Plan") }
    { Url ="/feedback"; Icon = feedbackSvg; Label = div [] (text "Feedback") }
    { Url ="/medications"; Icon = medicationsSvg; Label = div [] (text "My Medications") }
    { Url ="/forms"; Icon = formsSvg; Label = div [] (text "Forms") }
    { Url ="/education"; Icon = educationSvg; Label = div [] (text "Education") }
    { Url ="/proxies"; Icon = proxiesSvg; Label = div [] (text "My Proxies") }
  ]

let finder (t:Page) (activePageUrl:string) =
  let classAttr = 
    if t.Url = activePageUrl then
      ["class", "nav-item nav-item-active"]
    else
      ["class", "nav-item"]

  li classAttr [
    a t.Url [] [
      t.Icon
      t.Label
    ]
  ]

let getNavMenu (activePageUrl:string) =
  let allPages = pages |> List.map (fun t -> finder t activePageUrl)
  ul [] allPages    
let basePage url content =
  html [] [
    head [] [
      title [] "Home"
      script [ "src", "https://code.jquery.com/jquery-1.12.4.js"] []
      link [ "rel", "stylesheet"; "href", "https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css" ]
      script [ "src", "https://cdn.datatables.net/1.10.15/js/jquery.dataTables.js"] []
      link [ "rel", "stylesheet"; "href", "https://fonts.googleapis.com/css?family=Overpass" ]
      link [ "rel", "stylesheet"; "href", "content/site.css"; "type", "text/css" ]
      link [ "rel", "stylesheet"; "href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"; ]
      script [ "src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"] []
      meta [ "charset", "utf-8"]
      meta [ "name", "viewport"; "content", "width=device-width, initial-scale=1"]
    ]
    body [] [
      div ["class", "wrapper"] [
        div ["class", "main-logo"] [
          img ["src", "content/logo.svg"; "width", "130px"]
        ]
        div ["class", "main-info"] [
          patientDisplayTopRight
          a "/logout" ["class", "btn btn-default"] (text "Logout")
        ]
        nav ["class", "main-nav"] [
          getNavMenu url
        ]
        div ["class", "main-sidebar"] [
          p [] (text "Stevens, Joseph")
        ]
        div ["class", "main-content"] [
          content
        ]
      ]
    ]
  ]
  |> htmlToString