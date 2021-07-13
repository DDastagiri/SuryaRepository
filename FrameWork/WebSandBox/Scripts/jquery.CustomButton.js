//CustomButton (div or button)
(function($) {
	var pluginName = "CustomButton",
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
			iconUrl: null,
			label: null,
			click: null
		},
			
		create: function(options, elem) {
			//constructor
			var self = this,
				element = $(elem);
				
			this.originalElement = element;
			this.options = $.extend(true, {}, this.options, options);
				
			element
				.bind("click."+pluginName, function(e) {
					if (!self.disabled()) {
						element.addClass("icrop-pressed");

						setTimeout(function() {
							element.removeClass("icrop-pressed");
							if (options.click) {
								options.click(e);
							}
						}, 300);
					}
				})
				.addClass("icrop-CustomButton")
				.css("position", "relative");
			
			this._badgeCount = 0;
			this.badgeElement = $("<div class='icrop-CustomButton-badge' style='position: absolute;text-align:center; z-index:1;'>0</div>");
			this.badgeElement
				.appendTo(element)
				.hide();
			
			if (options.label) {
				$("<div class='icrop-CustomButton-label' style='position:absolute;text-align:center;'></div>")
					.text(options.label)
					.width(element.width())
					.appendTo(element);
			}
			if (options.iconUrl) {
				element.css("background-image", "url(" + options.iconUrl + ")");
			}
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
		},
		
		badgeCount: function(value) {
			if (value === undefined) {
				//getter
				return this._badgeCount;
			} else {
				//setter
				this._badgeCount = value;
				if (this._badgeCount <= 0) {
					this.badgeElement.hide();
				} else {
					this.badgeElement.text("" + this._badgeCount).show();
				}
			}
		}
		
	};
	pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);