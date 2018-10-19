(function($) {
	
	$(document).ready(function(event) {
		
		if(wpgmza_ajax_fix.request_uri.match(/index/))
			return;
		
		setInterval(function() {
			var elems = $("#map");
			
			if(!elems.length)
				return;
			
			var el = elems[0];
			
			if(!el.firstElementChild)
				InitMap(1,'all',false);
			
		}, 1000);
		
	});
	
})(jQuery);