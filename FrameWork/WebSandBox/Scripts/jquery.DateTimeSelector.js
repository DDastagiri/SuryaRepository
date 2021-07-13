//DateTimeSelector
(function($) {
	var pluginName = "DateTimeSelector",
		pluginImpl;
	
	$.fn[pluginName] = pluginImpl =  function(options) {
		if (typeof options == "string") {
			// method call
			var args = Array.prototype.slice.call(arguments, 1),
				returnValue = this;
			this.each(function() {
				var instance = $.data(this, pluginName),				
					value = (instance && $.isFunction(instance[options])) ? instance[options].apply(instance, args) : instance;
				if (value !== instance && value !== undefined) {
					returnValue = value;
					return false;
				}
			});					
			return returnValue;
		} else {
			// constructor call (create or init)
			return this.each(function() {
				var instance = $.data(this, pluginName);   
				if (instance) {
					$.extend(true, instance.options, options)
					instance.init();
				} else {
					$.data(this, pluginName, new pluginImpl.prototype.create(options, this));
				}
			});
		}
	};
	
	pluginImpl.prototype = {
		options: {
			//default option values 
		},
			
		create: function(options, elem) {
			//constructor
			var self = this,
				element = $(elem);
				
			this.originalElement = element;
			this.options = $.extend(true, {}, this.options, options);
  
			element
				.bind("focusin."+pluginName, function(e) {
					$(this).addClass('icrop-focused');						
				})
				.bind("focusout."+pluginName, function(e) {
					$(this).removeClass('icrop-focused');
				})
				.addClass("icrop-DateTimeSelector");
		},
			
		init: function() {
			//reload options
		},
		
		disabled: function(value) {
			if (value === undefined) {
				//getter
				return this.originalElement.hasClass("icrop-disabled");
			} else {
				//setter
				if (value === true) {
					this.originalElement
						.addClass("icrop-disabled")
						.attr("disabled", "disabled");
				} else {
					this.originalElement
						.removeClass("icrop-disabled")
						.removeAttr("disabled");
				}
			}
		}
	};
	pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);