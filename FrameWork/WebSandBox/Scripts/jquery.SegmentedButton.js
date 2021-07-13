//SegmentedButton (ul > li > radio)
(function($) {
	var pluginName = "SegmentedButton",
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
			formName: null,
			select: null
		},
			
		create: function(options, elem) {
			//constructor
			var self = this,
				element = $(elem);
				
			this.originalElement = element;
			this.options = $.extend(true, {}, this.options, options);
				
			element.addClass("icrop-SegmentedButton");
			element.children("li").click(function(e) {
				var originalValue = $("input[name='" + self.options.formName + "']:checked").val(),
					newValue = $(this).children("input").attr("value");
				if (self.disabled() || originalValue == newValue) {
					return;
				}

	            $("input[name='" + self.options.formName + "']").val([newValue]);
				
				element.children("li").removeClass("icrop-selected");
				$(this).addClass("icrop-selected");
				if (self.options.select) {
					self.options.select(newValue);
				}
			});
			element.children("li").children("input").css("-webkit-appearance", "none");

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
						.children("li").addClass("icrop-disabled");
				} else {
					this.originalElement
						.removeClass("icrop-disabled")
						.children("li").removeClass("icrop-disabled");
				}
			}
		}
		
	};
	pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);