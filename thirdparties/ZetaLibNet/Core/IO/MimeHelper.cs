namespace ZetaLib.Core.IO
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Runtime.InteropServices;
	using System.Diagnostics;
	using System.Reflection;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper methods for MIME types.
	/// </summary>
	public sealed class MimeHelper
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Mapps a file extension to a mime type.
		/// </summary>
		/// <param name="fileExtension">The file extension.</param>
		/// <returns></returns>
		public static string MapFileExtenstionToMimeType(
			string fileExtension )
		{
			CheckFillMappingDictionary();

			fileExtension = fileExtension.Trim( '.' ).ToLowerInvariant();

			if ( mappings.ContainsKey( fileExtension ) )
			{
				return mappings[fileExtension];
			}
			else
			{
				return string.Format(
				   @"application/{0}",
				   fileExtension );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks the fill mapping dictionary.
		/// </summary>
		private static void CheckFillMappingDictionary()
		{
			if ( mappings == null )
			{
				mappings = new Dictionary<string, string>();

				// From Hannes Dorbarth from Imos (hd@imos.net).
				mappings[@"pdf"] = @"application/pdf";
				mappings[@"sig"] = @"application/pgp-signature";
				mappings[@"spl"] = @"application/futuresplash";
				mappings[@"class"] = @"application/octet-stream";
				mappings[@"ps"] = @"application/postscript";
				mappings[@"torrent"] = @"application/x-bittorrent";
				mappings[@"dvi"] = @"application/x-dvi";
				mappings[@"gz"] = @"application/x-gzip";
				mappings[@"pac"] = @"application/x-ns-proxy-autoconfig";
				mappings[@"swf"] = @"application/x-shockwave-flash";
				mappings[@"tar.gz"] = @"application/x-tgz";
				mappings[@"tgz"] = @"application/x-tgz";
				mappings[@"tar"] = @"application/x-tar";
				mappings[@"zip"] = @"application/zip";
				mappings[@"mp3"] = @"audio/mpeg";
				mappings[@"m3u"] = @"audio/x-mpegurl";
				mappings[@"wma"] = @"audio/x-ms-wma";
				mappings[@"wax"] = @"audio/x-ms-wax";
				mappings[@"ogg"] = @"application/ogg";
				mappings[@"wav"] = @"audio/x-wav";
				mappings[@"gif"] = @"image/gif";
				mappings[@"jpg"] = @"image/jpeg";
				mappings[@"jpeg"] = @"image/jpeg";
				mappings[@"png"] = @"image/png";
				mappings[@"xbm"] = @"image/x-xbitmap";
				mappings[@"xpm"] = @"image/x-xpixmap";
				mappings[@"xwd"] = @"image/x-xwindowdump";
				mappings[@"css"] = @"text/css";
				mappings[@"html"] = @"text/html";
				mappings[@"htm"] = @"text/html";
				mappings[@"js"] = @"text/javascript";
				mappings[@"asc"] = @"text/plain";
				mappings[@"c"] = @"text/plain";
				mappings[@"cpp"] = @"text/plain";
				mappings[@"log"] = @"text/plain";
				mappings[@"conf"] = @"text/plain";
				mappings[@"text"] = @"text/plain";
				mappings[@"txt"] = @"text/plain";
				mappings[@"dtd"] = @"text/xml";
				mappings[@"xml"] = @"text/xml";
				mappings[@"mpeg"] = @"video/mpeg";
				mappings[@"mpg"] = @"video/mpeg";
				mappings[@"mov"] = @"video/quicktime";
				mappings[@"qt"] = @"video/quicktime";
				mappings[@"avi"] = @"video/x-msvideo";
				mappings[@"asf"] = @"video/x-ms-asf";
				mappings[@"asx"] = @"video/x-ms-asf";
				mappings[@"wmv"] = @"video/x-ms-wmv";
				mappings[@"bz2"] = @"application/x-bzip";
				mappings[@"tbz"] = @"application/x-bzip-compressed-tar";
				mappings[@"tar.bz2"] = @"application/x-bzip-compressed-tar";

				// From http://www.starttipp.de/verschiedenes/mime-types.html.
				mappings[@"3dm"] = @"x-world/x-3dmf";
				mappings[@"3dmf"] = @"x-world/x-3dmf";
				mappings[@"a"] = @"application/octet-stream";
				mappings[@"aab"] = @"application/x-authorware-bin";
				mappings[@"aam"] = @"application/x-authorware-map";
				mappings[@"aas"] = @"application/x-authorware-seg";
				mappings[@"abc"] = @"text/vnd.abc";
				mappings[@"acgi"] = @"text/html";
				mappings[@"afl"] = @"video/animaflex";
				mappings[@"ai"] = @"application/postscript";
				mappings[@"aif"] = @"audio/aiff";
				mappings[@"aif"] = @"audio/x-aiff";
				mappings[@"aifc"] = @"audio/aiff";
				mappings[@"aifc"] = @"audio/x-aiff";
				mappings[@"aiff"] = @"audio/aiff";
				mappings[@"aiff"] = @"audio/x-aiff";
				mappings[@"aim"] = @"application/x-aim";
				mappings[@"aip"] = @"text/x-audiosoft-intra";
				mappings[@"ani"] = @"application/x-navi-animation";
				mappings[@"aos"] = @"application/x-nokia-9000-communicator-add-on-software";
				mappings[@"aps"] = @"application/mime";
				mappings[@"arc"] = @"application/octet-stream";
				mappings[@"arj"] = @"application/arj";
				mappings[@"arj"] = @"application/octet-stream";
				mappings[@"art"] = @"image/x-jg";
				mappings[@"asf"] = @"video/x-ms-asf";
				mappings[@"asm"] = @"text/x-asm";
				mappings[@"asp"] = @"text/asp";
				mappings[@"asx"] = @"application/x-mplayer2";
				mappings[@"asx"] = @"video/x-ms-asf";
				mappings[@"asx"] = @"video/x-ms-asf-plugin";
				mappings[@"au"] = @"audio/basic";
				mappings[@"au"] = @"audio/x-au";
				mappings[@"avi"] = @"application/x-troff-msvideo";
				mappings[@"avi"] = @"video/avi";
				mappings[@"avi"] = @"video/msvideo";
				mappings[@"avi"] = @"video/x-msvideo";
				mappings[@"avs"] = @"video/avs-video";
				mappings[@"bcpio"] = @"application/x-bcpio";
				mappings[@"bin"] = @"application/mac-binary";
				mappings[@"bin"] = @"application/macbinary";
				mappings[@"bin"] = @"application/octet-stream";
				mappings[@"bin"] = @"application/x-binary";
				mappings[@"bin"] = @"application/x-macbinary";
				mappings[@"bm"] = @"image/bmp";
				mappings[@"bmp"] = @"image/bmp";
				mappings[@"bmp"] = @"image/x-windows-bmp";
				mappings[@"boo"] = @"application/book";
				mappings[@"book"] = @"application/book";
				mappings[@"boz"] = @"application/x-bzip2";
				mappings[@"bsh"] = @"application/x-bsh";
				mappings[@"bz"] = @"application/x-bzip";
				mappings[@"bz2"] = @"application/x-bzip2";
				mappings[@"c"] = @"text/plain";
				mappings[@"c"] = @"text/x-c";
				mappings[@"c++"] = @"text/plain";
				mappings[@"cat"] = @"application/vnd.ms-pki.seccat";
				mappings[@"cc"] = @"text/plain";
				mappings[@"cc"] = @"text/x-c";
				mappings[@"ccad"] = @"application/clariscad";
				mappings[@"cco"] = @"application/x-cocoa";
				mappings[@"cdf"] = @"application/cdf";
				mappings[@"cdf"] = @"application/x-cdf";
				mappings[@"cdf"] = @"application/x-netcdf";
				mappings[@"cer"] = @"application/pkix-cert";
				mappings[@"cer"] = @"application/x-x509-ca-cert";
				mappings[@"cha"] = @"application/x-chat";
				mappings[@"chat"] = @"application/x-chat";
				mappings[@"class"] = @"application/java";
				mappings[@"class"] = @"application/java-byte-code";
				mappings[@"class"] = @"application/x-java-class";
				mappings[@"com"] = @"application/octet-stream";
				mappings[@"com"] = @"text/plain";
				mappings[@"conf"] = @"text/plain";
				mappings[@"cpio"] = @"application/x-cpio";
				mappings[@"cpp"] = @"text/x-c";
				mappings[@"cpt"] = @"application/mac-compactpro";
				mappings[@"cpt"] = @"application/x-compactpro";
				mappings[@"cpt"] = @"application/x-cpt";
				mappings[@"crl"] = @"application/pkcs-crl";
				mappings[@"crl"] = @"application/pkix-crl";
				mappings[@"crt"] = @"application/pkix-cert";
				mappings[@"crt"] = @"application/x-x509-ca-cert";
				mappings[@"crt"] = @"application/x-x509-user-cert";
				mappings[@"csh"] = @"application/x-csh";
				mappings[@"csh"] = @"text/x-script.csh";
				mappings[@"css"] = @"application/x-pointplus";
				mappings[@"css"] = @"text/css";
				mappings[@"cxx"] = @"text/plain";
				mappings[@"dcr"] = @"application/x-director";
				mappings[@"deepv"] = @"application/x-deepv";
				mappings[@"def"] = @"text/plain";
				mappings[@"der"] = @"application/x-x509-ca-cert";
				mappings[@"dif"] = @"video/x-dv";
				mappings[@"dir"] = @"application/x-director";
				mappings[@"dl"] = @"video/dl";
				mappings[@"dl"] = @"video/x-dl";
				mappings[@"doc"] = @"application/msword";
				mappings[@"dot"] = @"application/msword";
				mappings[@"dp"] = @"application/commonground";
				mappings[@"drw"] = @"application/drafting";
				mappings[@"dump"] = @"application/octet-stream";
				mappings[@"dv"] = @"video/x-dv";
				mappings[@"dvi"] = @"application/x-dvi";
				mappings[@"dwf"] = @"drawing/x-dwf (old)";
				mappings[@"dwf"] = @"model/vnd.dwf";
				mappings[@"dwg"] = @"application/acad";
				mappings[@"dwg"] = @"image/vnd.dwg";
				mappings[@"dwg"] = @"image/x-dwg";
				mappings[@"dxf"] = @"application/dxf";
				mappings[@"dxf"] = @"image/vnd.dwg";
				mappings[@"dxf"] = @"image/x-dwg";
				mappings[@"dxr"] = @"application/x-director";
				mappings[@"el"] = @"text/x-script.elisp";
				mappings[@"elc"] = @"application/x-bytecode.elisp (compiled elisp)";
				mappings[@"elc"] = @"application/x-elc";
				mappings[@"env"] = @"application/x-envoy";
				mappings[@"eps"] = @"application/postscript";
				mappings[@"es"] = @"application/x-esrehber";
				mappings[@"etx"] = @"text/x-setext";
				mappings[@"evy"] = @"application/envoy";
				mappings[@"evy"] = @"application/x-envoy";
				mappings[@"exe"] = @"application/octet-stream";
				mappings[@"f"] = @"text/plain";
				mappings[@"f"] = @"text/x-fortran";
				mappings[@"f77"] = @"text/x-fortran";
				mappings[@"f90"] = @"text/plain";
				mappings[@"f90"] = @"text/x-fortran";
				mappings[@"fdf"] = @"application/vnd.fdf";
				mappings[@"fif"] = @"application/fractals";
				mappings[@"fif"] = @"image/fif";
				mappings[@"fli"] = @"video/fli";
				mappings[@"fli"] = @"video/x-fli";
				mappings[@"flo"] = @"image/florian";
				mappings[@"flx"] = @"text/vnd.fmi.flexstor";
				mappings[@"fmf"] = @"video/x-atomic3d-feature";
				mappings[@"for"] = @"text/plain";
				mappings[@"for"] = @"text/x-fortran";
				mappings[@"fpx"] = @"image/vnd.fpx";
				mappings[@"fpx"] = @"image/vnd.net-fpx";
				mappings[@"frl"] = @"application/freeloader";
				mappings[@"funk"] = @"audio/make";
				mappings[@"g"] = @"text/plain";
				mappings[@"g3"] = @"image/g3fax";
				mappings[@"gif"] = @"image/gif";
				mappings[@"gl"] = @"video/gl";
				mappings[@"gl"] = @"video/x-gl";
				mappings[@"gsd"] = @"audio/x-gsm";
				mappings[@"gsm"] = @"audio/x-gsm";
				mappings[@"gsp"] = @"application/x-gsp";
				mappings[@"gss"] = @"application/x-gss";
				mappings[@"gtar"] = @"application/x-gtar";
				mappings[@"gz"] = @"application/x-compressed";
				mappings[@"gz"] = @"application/x-gzip";
				mappings[@"gzip"] = @"application/x-gzip";
				mappings[@"gzip"] = @"multipart/x-gzip";
				mappings[@"h"] = @"text/plain";
				mappings[@"h"] = @"text/x-h";
				mappings[@"hdf"] = @"application/x-hdf";
				mappings[@"help"] = @"application/x-helpfile";
				mappings[@"hgl"] = @"application/vnd.hp-hpgl";
				mappings[@"hh"] = @"text/plain";
				mappings[@"hh"] = @"text/x-h";
				mappings[@"hlb"] = @"text/x-script";
				mappings[@"hlp"] = @"application/hlp";
				mappings[@"hlp"] = @"application/x-helpfile";
				mappings[@"hlp"] = @"application/x-winhelp";
				mappings[@"hpg"] = @"application/vnd.hp-hpgl";
				mappings[@"hpgl"] = @"application/vnd.hp-hpgl";
				mappings[@"hqx"] = @"application/binhex";
				mappings[@"hqx"] = @"application/binhex4";
				mappings[@"hqx"] = @"application/mac-binhex";
				mappings[@"hqx"] = @"application/mac-binhex40";
				mappings[@"hqx"] = @"application/x-binhex40";
				mappings[@"hqx"] = @"application/x-mac-binhex40";
				mappings[@"hta"] = @"application/hta";
				mappings[@"htc"] = @"text/x-component";
				mappings[@"htm"] = @"text/html";
				mappings[@"html"] = @"text/html";
				mappings[@"htmls"] = @"text/html";
				mappings[@"htt"] = @"text/webviewhtml";
				mappings[@"htx"] = @"text/html";
				mappings[@"ice"] = @"x-conference/x-cooltalk";
				mappings[@"ico"] = @"image/x-icon";
				mappings[@"idc"] = @"text/plain";
				mappings[@"ief"] = @"image/ief";
				mappings[@"iefs"] = @"image/ief";
				mappings[@"iges"] = @"application/iges";
				mappings[@"iges"] = @"model/iges";
				mappings[@"igs"] = @"application/iges";
				mappings[@"igs"] = @"model/iges";
				mappings[@"ima"] = @"application/x-ima";
				mappings[@"imap"] = @"application/x-httpd-imap";
				mappings[@"inf"] = @"application/inf";
				mappings[@"ins"] = @"application/x-internett-signup";
				mappings[@"ip"] = @"application/x-ip2";
				mappings[@"isu"] = @"video/x-isvideo";
				mappings[@"it"] = @"audio/it";
				mappings[@"iv"] = @"application/x-inventor";
				mappings[@"ivr"] = @"i-world/i-vrml";
				mappings[@"ivy"] = @"application/x-livescreen";
				mappings[@"jam"] = @"audio/x-jam";
				mappings[@"jav"] = @"text/plain";
				mappings[@"jav"] = @"text/x-java-source";
				mappings[@"java"] = @"text/plain";
				mappings[@"java"] = @"text/x-java-source";
				mappings[@"jcm"] = @"application/x-java-commerce";
				mappings[@"jfif"] = @"image/jpeg";
				mappings[@"jfif"] = @"image/pjpeg";
				mappings[@"jfif-tbnl"] = @"image/jpeg";
				mappings[@"jpe"] = @"image/jpeg";
				mappings[@"jpe"] = @"image/pjpeg";
				mappings[@"jpeg"] = @"image/jpeg";
				mappings[@"jpeg"] = @"image/pjpeg";
				mappings[@"jpg"] = @"image/jpeg";
				mappings[@"jpg"] = @"image/pjpeg";
				mappings[@"jps"] = @"image/x-jps";
				mappings[@"js"] = @"application/x-javascript";
				mappings[@"jut"] = @"image/jutvision";
				mappings[@"kar"] = @"audio/midi";
				mappings[@"kar"] = @"music/x-karaoke";
				mappings[@"ksh"] = @"application/x-ksh";
				mappings[@"ksh"] = @"text/x-script.ksh";
				mappings[@"la"] = @"audio/nspaudio";
				mappings[@"la"] = @"audio/x-nspaudio";
				mappings[@"lam"] = @"audio/x-liveaudio";
				mappings[@"latex"] = @"application/x-latex";
				mappings[@"lha"] = @"application/lha";
				mappings[@"lha"] = @"application/octet-stream";
				mappings[@"lha"] = @"application/x-lha";
				mappings[@"lhx"] = @"application/octet-stream";
				mappings[@"list"] = @"text/plain";
				mappings[@"lma"] = @"audio/nspaudio";
				mappings[@"lma"] = @"audio/x-nspaudio";
				mappings[@"log"] = @"text/plain";
				mappings[@"lsp"] = @"application/x-lisp";
				mappings[@"lsp"] = @"text/x-script.lisp";
				mappings[@"lst"] = @"text/plain";
				mappings[@"lsx"] = @"text/x-la-asf";
				mappings[@"ltx"] = @"application/x-latex";
				mappings[@"lzh"] = @"application/octet-stream";
				mappings[@"lzh"] = @"application/x-lzh";
				mappings[@"lzx"] = @"application/lzx";
				mappings[@"lzx"] = @"application/octet-stream";
				mappings[@"lzx"] = @"application/x-lzx";
				mappings[@"m"] = @"text/plain";
				mappings[@"m"] = @"text/x-m";
				mappings[@"m1v"] = @"video/mpeg";
				mappings[@"m2a"] = @"audio/mpeg";
				mappings[@"m2v"] = @"video/mpeg";
				mappings[@"m3u"] = @"audio/x-mpequrl";
				mappings[@"man"] = @"application/x-troff-man";
				mappings[@"map"] = @"application/x-navimap";
				mappings[@"mar"] = @"text/plain";
				mappings[@"mbd"] = @"application/mbedlet";
				mappings[@"mc$"] = @"application/x-magic-cap-package-1.0";
				mappings[@"mcd"] = @"application/mcad";
				mappings[@"mcd"] = @"application/x-mathcad";
				mappings[@"mcf"] = @"image/vasa";
				mappings[@"mcf"] = @"text/mcf";
				mappings[@"mcp"] = @"application/netmc";
				mappings[@"me"] = @"application/x-troff-me";
				mappings[@"mht"] = @"message/rfc822";
				mappings[@"mhtml"] = @"message/rfc822";
				mappings[@"mid"] = @"application/x-midi";
				mappings[@"mid"] = @"audio/midi";
				mappings[@"mid"] = @"audio/x-mid";
				mappings[@"mid"] = @"audio/x-midi";
				mappings[@"mid"] = @"music/crescendo";
				mappings[@"mid"] = @"x-music/x-midi";
				mappings[@"midi"] = @"application/x-midi";
				mappings[@"midi"] = @"audio/midi";
				mappings[@"midi"] = @"audio/x-mid";
				mappings[@"midi"] = @"audio/x-midi";
				mappings[@"midi"] = @"music/crescendo";
				mappings[@"midi"] = @"x-music/x-midi";
				mappings[@"mif"] = @"application/x-frame";
				mappings[@"mif"] = @"application/x-mif";
				mappings[@"mime"] = @"message/rfc822";
				mappings[@"mime"] = @"www/mime";
				mappings[@"mjf"] = @"audio/x-vnd.audioexplosion.mjuicemediafile";
				mappings[@"mjpg"] = @"video/x-motion-jpeg";
				mappings[@"mm"] = @"application/base64";
				mappings[@"mm"] = @"application/x-meme";
				mappings[@"mme"] = @"application/base64";
				mappings[@"mod"] = @"audio/mod";
				mappings[@"mod"] = @"audio/x-mod";
				mappings[@"moov"] = @"video/quicktime";
				mappings[@"mov"] = @"video/quicktime";
				mappings[@"movie"] = @"video/x-sgi-movie";
				mappings[@"mp2"] = @"audio/mpeg";
				mappings[@"mp2"] = @"audio/x-mpeg";
				mappings[@"mp2"] = @"video/mpeg";
				mappings[@"mp2"] = @"video/x-mpeg";
				mappings[@"mp2"] = @"video/x-mpeq2a";
				mappings[@"mp3"] = @"audio/mpeg3";
				mappings[@"mp3"] = @"audio/x-mpeg-3";
				mappings[@"mp3"] = @"video/mpeg";
				mappings[@"mp3"] = @"video/x-mpeg";
				mappings[@"mpa"] = @"audio/mpeg";
				mappings[@"mpa"] = @"video/mpeg";
				mappings[@"mpc"] = @"application/x-project";
				mappings[@"mpe"] = @"video/mpeg";
				mappings[@"mpeg"] = @"video/mpeg";
				mappings[@"mpg"] = @"audio/mpeg";
				mappings[@"mpg"] = @"video/mpeg";
				mappings[@"mpga"] = @"audio/mpeg";
				mappings[@"mpp"] = @"application/vnd.ms-project";
				mappings[@"mpt"] = @"application/x-project";
				mappings[@"mpv"] = @"application/x-project";
				mappings[@"mpx"] = @"application/x-project";
				mappings[@"mrc"] = @"application/marc";
				mappings[@"ms"] = @"application/x-troff-ms";
				mappings[@"mv"] = @"video/x-sgi-movie";
				mappings[@"my"] = @"audio/make";
				mappings[@"mzz"] = @"application/x-vnd.audioexplosion.mzz";
				mappings[@"nap"] = @"image/naplps";
				mappings[@"naplps"] = @"image/naplps";
				mappings[@"nc"] = @"application/x-netcdf";
				mappings[@"ncm"] = @"application/vnd.nokia.configuration-message";
				mappings[@"nif"] = @"image/x-niff";
				mappings[@"niff"] = @"image/x-niff";
				mappings[@"nix"] = @"application/x-mix-transfer";
				mappings[@"nsc"] = @"application/x-conference";
				mappings[@"nvd"] = @"application/x-navidoc";
				mappings[@"o"] = @"application/octet-stream";
				mappings[@"oda"] = @"application/oda";
				mappings[@"omc"] = @"application/x-omc";
				mappings[@"omcd"] = @"application/x-omcdatamaker";
				mappings[@"omcr"] = @"application/x-omcregerator";
				mappings[@"p"] = @"text/x-pascal";
				mappings[@"p10"] = @"application/pkcs10";
				mappings[@"p10"] = @"application/x-pkcs10";
				mappings[@"p12"] = @"application/pkcs-12";
				mappings[@"p12"] = @"application/x-pkcs12";
				mappings[@"p7a"] = @"application/x-pkcs7-signature";
				mappings[@"p7c"] = @"application/pkcs7-mime";
				mappings[@"p7c"] = @"application/x-pkcs7-mime";
				mappings[@"p7m"] = @"application/pkcs7-mime";
				mappings[@"p7m"] = @"application/x-pkcs7-mime";
				mappings[@"p7r"] = @"application/x-pkcs7-certreqresp";
				mappings[@"p7s"] = @"application/pkcs7-signature";
				mappings[@"part"] = @"application/pro_eng";
				mappings[@"pas"] = @"text/pascal";
				mappings[@"pbm"] = @"image/x-portable-bitmap";
				mappings[@"pcl"] = @"application/vnd.hp-pcl";
				mappings[@"pcl"] = @"application/x-pcl";
				mappings[@"pct"] = @"image/x-pict";
				mappings[@"pcx"] = @"image/x-pcx";
				mappings[@"pdb"] = @"chemical/x-pdb";
				mappings[@"pdf"] = @"application/pdf";
				mappings[@"pfunk"] = @"audio/make";
				mappings[@"pfunk"] = @"audio/make.my.funk";
				mappings[@"pgm"] = @"image/x-portable-graymap";
				mappings[@"pgm"] = @"image/x-portable-greymap";
				mappings[@"pic"] = @"image/pict";
				mappings[@"pict"] = @"image/pict";
				mappings[@"pkg"] = @"application/x-newton-compatible-pkg";
				mappings[@"pko"] = @"application/vnd.ms-pki.pko";
				mappings[@"pl"] = @"text/plain";
				mappings[@"pl"] = @"text/x-script.perl";
				mappings[@"plx"] = @"application/x-pixclscript";
				mappings[@"pm"] = @"image/x-xpixmap";
				mappings[@"pm"] = @"text/x-script.perl-module";
				mappings[@"pm4"] = @"application/x-pagemaker";
				mappings[@"pm5"] = @"application/x-pagemaker";
				mappings[@"png"] = @"image/png";
				mappings[@"pnm"] = @"application/x-portable-anymap";
				mappings[@"pnm"] = @"image/x-portable-anymap";
				mappings[@"pot"] = @"application/mspowerpoint";
				mappings[@"pot"] = @"application/vnd.ms-powerpoint";
				mappings[@"pov"] = @"model/x-pov";
				mappings[@"ppa"] = @"application/vnd.ms-powerpoint";
				mappings[@"ppm"] = @"image/x-portable-pixmap";
				mappings[@"pps"] = @"application/mspowerpoint";
				mappings[@"pps"] = @"application/vnd.ms-powerpoint";
				mappings[@"ppt"] = @"application/mspowerpoint";
				mappings[@"ppt"] = @"application/powerpoint";
				mappings[@"ppt"] = @"application/vnd.ms-powerpoint";
				mappings[@"ppt"] = @"application/x-mspowerpoint";
				mappings[@"ppz"] = @"application/mspowerpoint";
				mappings[@"pre"] = @"application/x-freelance";
				mappings[@"prt"] = @"application/pro_eng";
				mappings[@"ps"] = @"application/postscript";
				mappings[@"psd"] = @"application/octet-stream";
				mappings[@"pvu"] = @"paleovu/x-pv";
				mappings[@"pwz"] = @"application/vnd.ms-powerpoint";
				mappings[@"py"] = @"text/x-script.phyton";
				mappings[@"pyc"] = @"applicaiton/x-bytecode.python";
				mappings[@"qcp"] = @"audio/vnd.qcelp";
				mappings[@"qd3"] = @"x-world/x-3dmf";
				mappings[@"qd3d"] = @"x-world/x-3dmf";
				mappings[@"qif"] = @"image/x-quicktime";
				mappings[@"qt"] = @"video/quicktime";
				mappings[@"qtc"] = @"video/x-qtc";
				mappings[@"qti"] = @"image/x-quicktime";
				mappings[@"qtif"] = @"image/x-quicktime";
				mappings[@"ra"] = @"audio/x-pn-realaudio";
				mappings[@"ra"] = @"audio/x-pn-realaudio-plugin";
				mappings[@"ra"] = @"audio/x-realaudio";
				mappings[@"ram"] = @"audio/x-pn-realaudio";
				mappings[@"ras"] = @"application/x-cmu-raster";
				mappings[@"ras"] = @"image/cmu-raster";
				mappings[@"ras"] = @"image/x-cmu-raster";
				mappings[@"rast"] = @"image/cmu-raster";
				mappings[@"rexx"] = @"text/x-script.rexx";
				mappings[@"rf"] = @"image/vnd.rn-realflash";
				mappings[@"rgb"] = @"image/x-rgb";
				mappings[@"rm"] = @"application/vnd.rn-realmedia";
				mappings[@"rm"] = @"audio/x-pn-realaudio";
				mappings[@"rmi"] = @"audio/mid";
				mappings[@"rmm"] = @"audio/x-pn-realaudio";
				mappings[@"rmp"] = @"audio/x-pn-realaudio";
				mappings[@"rmp"] = @"audio/x-pn-realaudio-plugin";
				mappings[@"rng"] = @"application/ringing-tones";
				mappings[@"rng"] = @"application/vnd.nokia.ringing-tone";
				mappings[@"rnx"] = @"application/vnd.rn-realplayer";
				mappings[@"roff"] = @"application/x-troff";
				mappings[@"rp"] = @"image/vnd.rn-realpix";
				mappings[@"rpm"] = @"audio/x-pn-realaudio-plugin";
				mappings[@"rt"] = @"text/richtext";
				mappings[@"rt"] = @"text/vnd.rn-realtext";
				mappings[@"rtf"] = @"application/rtf";
				mappings[@"rtf"] = @"application/x-rtf";
				mappings[@"rtf"] = @"text/richtext";
				mappings[@"rtx"] = @"application/rtf";
				mappings[@"rtx"] = @"text/richtext";
				mappings[@"rv"] = @"video/vnd.rn-realvideo";
				mappings[@"s"] = @"text/x-asm";
				mappings[@"s3m"] = @"audio/s3m";
				mappings[@"saveme"] = @"application/octet-stream";
				mappings[@"sbk"] = @"application/x-tbook";
				mappings[@"scm"] = @"application/x-lotusscreencam";
				mappings[@"scm"] = @"text/x-script.guile";
				mappings[@"scm"] = @"text/x-script.scheme";
				mappings[@"scm"] = @"video/x-scm";
				mappings[@"sdml"] = @"text/plain";
				mappings[@"sdp"] = @"application/sdp";
				mappings[@"sdp"] = @"application/x-sdp";
				mappings[@"sdr"] = @"application/sounder";
				mappings[@"sea"] = @"application/sea";
				mappings[@"sea"] = @"application/x-sea";
				mappings[@"set"] = @"application/set";
				mappings[@"sgm"] = @"text/sgml";
				mappings[@"sgm"] = @"text/x-sgml";
				mappings[@"sgml"] = @"text/sgml";
				mappings[@"sgml"] = @"text/x-sgml";
				mappings[@"sh"] = @"application/x-bsh";
				mappings[@"sh"] = @"application/x-sh";
				mappings[@"sh"] = @"application/x-shar";
				mappings[@"sh"] = @"text/x-script.sh";
				mappings[@"shar"] = @"application/x-bsh";
				mappings[@"shar"] = @"application/x-shar";
				mappings[@"shtml"] = @"text/html";
				mappings[@"shtml"] = @"text/x-server-parsed-html";
				mappings[@"sid"] = @"audio/x-psid";
				mappings[@"sit"] = @"application/x-sit";
				mappings[@"sit"] = @"application/x-stuffit";
				mappings[@"skd"] = @"application/x-koan";
				mappings[@"skm"] = @"application/x-koan";
				mappings[@"skp"] = @"application/x-koan";
				mappings[@"skt"] = @"application/x-koan";
				mappings[@"sl"] = @"application/x-seelogo";
				mappings[@"smi"] = @"application/smil";
				mappings[@"smil"] = @"application/smil";
				mappings[@"snd"] = @"audio/basic";
				mappings[@"snd"] = @"audio/x-adpcm";
				mappings[@"sol"] = @"application/solids";
				mappings[@"spc"] = @"application/x-pkcs7-certificates";
				mappings[@"spc"] = @"text/x-speech";
				mappings[@"spl"] = @"application/futuresplash";
				mappings[@"spr"] = @"application/x-sprite";
				mappings[@"sprite"] = @"application/x-sprite";
				mappings[@"src"] = @"application/x-wais-source";
				mappings[@"ssi"] = @"text/x-server-parsed-html";
				mappings[@"ssm"] = @"application/streamingmedia";
				mappings[@"sst"] = @"application/vnd.ms-pki.certstore";
				mappings[@"step"] = @"application/step";
				mappings[@"stl"] = @"application/sla";
				mappings[@"stl"] = @"application/vnd.ms-pki.stl";
				mappings[@"stl"] = @"application/x-navistyle";
				mappings[@"stp"] = @"application/step";
				mappings[@"sv4cpio"] = @"application/x-sv4cpio";
				mappings[@"sv4crc"] = @"application/x-sv4crc";
				mappings[@"svf"] = @"image/vnd.dwg";
				mappings[@"svf"] = @"image/x-dwg";
				mappings[@"svr"] = @"application/x-world";
				mappings[@"svr"] = @"x-world/x-svr";
				mappings[@"swf"] = @"application/x-shockwave-flash";
				mappings[@"t"] = @"application/x-troff";
				mappings[@"talk"] = @"text/x-speech";
				mappings[@"tar"] = @"application/x-tar";
				mappings[@"tbk"] = @"application/toolbook";
				mappings[@"tbk"] = @"application/x-tbook";
				mappings[@"tcl"] = @"application/x-tcl";
				mappings[@"tcl"] = @"text/x-script.tcl";
				mappings[@"tcsh"] = @"text/x-script.tcsh";
				mappings[@"tex"] = @"application/x-tex";
				mappings[@"texi"] = @"application/x-texinfo";
				mappings[@"texinfo"] = @"application/x-texinfo";
				mappings[@"text"] = @"application/plain";
				mappings[@"text"] = @"text/plain";
				mappings[@"tgz"] = @"application/gnutar";
				mappings[@"tgz"] = @"application/x-compressed";
				mappings[@"tif"] = @"image/tiff";
				mappings[@"tif"] = @"image/x-tiff";
				mappings[@"tiff"] = @"image/tiff";
				mappings[@"tiff"] = @"image/x-tiff";
				mappings[@"tr"] = @"application/x-troff";
				mappings[@"tsi"] = @"audio/tsp-audio";
				mappings[@"tsp"] = @"application/dsptype";
				mappings[@"tsp"] = @"audio/tsplayer";
				mappings[@"tsv"] = @"text/tab-separated-values";
				mappings[@"turbot"] = @"image/florian";
				mappings[@"txt"] = @"text/plain";
				mappings[@"uil"] = @"text/x-uil";
				mappings[@"uni"] = @"text/uri-list";
				mappings[@"unis"] = @"text/uri-list";
				mappings[@"unv"] = @"application/i-deas";
				mappings[@"uri"] = @"text/uri-list";
				mappings[@"uris"] = @"text/uri-list";
				mappings[@"ustar"] = @"application/x-ustar";
				mappings[@"ustar"] = @"multipart/x-ustar";
				mappings[@"uu"] = @"application/octet-stream";
				mappings[@"uu"] = @"text/x-uuencode";
				mappings[@"uue"] = @"text/x-uuencode";
				mappings[@"vcd"] = @"application/x-cdlink";
				mappings[@"vcs"] = @"text/x-vcalendar";
				mappings[@"vda"] = @"application/vda";
				mappings[@"vdo"] = @"video/vdo";
				mappings[@"vew"] = @"application/groupwise";
				mappings[@"viv"] = @"video/vivo";
				mappings[@"viv"] = @"video/vnd.vivo";
				mappings[@"vivo"] = @"video/vivo";
				mappings[@"vivo"] = @"video/vnd.vivo";
				mappings[@"vmd"] = @"application/vocaltec-media-desc";
				mappings[@"vmf"] = @"application/vocaltec-media-file";
				mappings[@"voc"] = @"audio/voc";
				mappings[@"voc"] = @"audio/x-voc";
				mappings[@"vos"] = @"video/vosaic";
				mappings[@"vox"] = @"audio/voxware";
				mappings[@"vqe"] = @"audio/x-twinvq-plugin";
				mappings[@"vqf"] = @"audio/x-twinvq";
				mappings[@"vql"] = @"audio/x-twinvq-plugin";
				mappings[@"vrml"] = @"application/x-vrml";
				mappings[@"vrml"] = @"model/vrml";
				mappings[@"vrml"] = @"x-world/x-vrml";
				mappings[@"vrt"] = @"x-world/x-vrt";
				mappings[@"vsd"] = @"application/x-visio";
				mappings[@"vst"] = @"application/x-visio";
				mappings[@"vsw"] = @"application/x-visio";
				mappings[@"w60"] = @"application/wordperfect6.0";
				mappings[@"w61"] = @"application/wordperfect6.1";
				mappings[@"w6w"] = @"application/msword";
				mappings[@"wav"] = @"audio/wav";
				mappings[@"wav"] = @"audio/x-wav";
				mappings[@"wb1"] = @"application/x-qpro";
				mappings[@"wbmp"] = @"image/vnd.wap.wbmp";
				mappings[@"web"] = @"application/vnd.xara";
				mappings[@"wiz"] = @"application/msword";
				mappings[@"wk1"] = @"application/x-123";
				mappings[@"wmf"] = @"windows/metafile";
				mappings[@"wml"] = @"text/vnd.wap.wml";
				mappings[@"wmlc"] = @"application/vnd.wap.wmlc";
				mappings[@"wmls"] = @"text/vnd.wap.wmlscript";
				mappings[@"wmlsc"] = @"application/vnd.wap.wmlscriptc";
				mappings[@"word"] = @"application/msword";
				mappings[@"wp"] = @"application/wordperfect";
				mappings[@"wp5"] = @"application/wordperfect";
				mappings[@"wp5"] = @"application/wordperfect6.0";
				mappings[@"wp6"] = @"application/wordperfect";
				mappings[@"wpd"] = @"application/wordperfect";
				mappings[@"wpd"] = @"application/x-wpwin";
				mappings[@"wq1"] = @"application/x-lotus";
				mappings[@"wri"] = @"application/mswrite";
				mappings[@"wri"] = @"application/x-wri";
				mappings[@"wrl"] = @"application/x-world";
				mappings[@"wrl"] = @"model/vrml";
				mappings[@"wrl"] = @"x-world/x-vrml";
				mappings[@"wrz"] = @"model/vrml";
				mappings[@"wrz"] = @"x-world/x-vrml";
				mappings[@"wsc"] = @"text/scriplet";
				mappings[@"wsrc"] = @"application/x-wais-source";
				mappings[@"wtk"] = @"application/x-wintalk";
				mappings[@"xbm"] = @"image/x-xbitmap";
				mappings[@"xbm"] = @"image/x-xbm";
				mappings[@"xbm"] = @"image/xbm";
				mappings[@"xdr"] = @"video/x-amt-demorun";
				mappings[@"xgz"] = @"xgl/drawing";
				mappings[@"xif"] = @"image/vnd.xiff";
				mappings[@"xl"] = @"application/excel";
				mappings[@"xla"] = @"application/excel";
				mappings[@"xla"] = @"application/x-excel";
				mappings[@"xla"] = @"application/x-msexcel";
				mappings[@"xlb"] = @"application/excel";
				mappings[@"xlb"] = @"application/vnd.ms-excel";
				mappings[@"xlb"] = @"application/x-excel";
				mappings[@"xlc"] = @"application/excel";
				mappings[@"xlc"] = @"application/vnd.ms-excel";
				mappings[@"xlc"] = @"application/x-excel";
				mappings[@"xld"] = @"application/excel";
				mappings[@"xld"] = @"application/x-excel";
				mappings[@"xlk"] = @"application/excel";
				mappings[@"xlk"] = @"application/x-excel";
				mappings[@"xll"] = @"application/excel";
				mappings[@"xll"] = @"application/vnd.ms-excel";
				mappings[@"xll"] = @"application/x-excel";
				mappings[@"xlm"] = @"application/excel";
				mappings[@"xlm"] = @"application/vnd.ms-excel";
				mappings[@"xlm"] = @"application/x-excel";
				mappings[@"xls"] = @"application/excel";
				mappings[@"xls"] = @"application/vnd.ms-excel";
				mappings[@"xls"] = @"application/x-excel";
				mappings[@"xls"] = @"application/x-msexcel";
				mappings[@"xlt"] = @"application/excel";
				mappings[@"xlt"] = @"application/x-excel";
				mappings[@"xlv"] = @"application/excel";
				mappings[@"xlv"] = @"application/x-excel";
				mappings[@"xlw"] = @"application/excel";
				mappings[@"xlw"] = @"application/vnd.ms-excel";
				mappings[@"xlw"] = @"application/x-excel";
				mappings[@"xlw"] = @"application/x-msexcel";
				mappings[@"xm"] = @"audio/xm";
				mappings[@"xml"] = @"application/xml";
				mappings[@"xml"] = @"text/xml";
				mappings[@"xmz"] = @"xgl/movie";
				mappings[@"xpix"] = @"application/x-vnd.ls-xpix";
				mappings[@"xpm"] = @"image/x-xpixmap";
				mappings[@"xpm"] = @"image/xpm";
				mappings[@"x-png"] = @"image/png";
				mappings[@"xsr"] = @"video/x-amt-showrun";
				mappings[@"xwd"] = @"image/x-xwd";
				mappings[@"xwd"] = @"image/x-xwindowdump";
				mappings[@"xyz"] = @"chemical/x-pdb";
				mappings[@"z"] = @"application/x-compress";
				mappings[@"z"] = @"application/x-compressed";
				mappings[@"zip"] = @"application/x-compressed";
				mappings[@"zip"] = @"application/x-zip-compressed";
				mappings[@"zip"] = @"application/zip";
				mappings[@"zoo"] = @"application/octet-stream";
				mappings[@"zsh"] = @"text/x-script.zsh";
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static Dictionary<string, string> mappings;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}