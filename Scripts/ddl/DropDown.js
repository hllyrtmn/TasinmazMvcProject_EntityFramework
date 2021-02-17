//Creator: Jesper Norr

+function ($) {
    'use strict';

    var dataToggle = '[data-toggle="dropdown"]'
    var $change = $('.dropdown .dropdown-menu > li > a')
    var ddl = function (element) {
        $(element).on('click.jn.dropdown', this.toggle)
    }

    // Opens and Closes Dropdown
    ddl.prototype.toggle = function (e) {
        var $this = $(this)

        if ($this.is('.disabled, :disabled')) return

        var $parent = getParent($this)
        var active = $parent.hasClass('open')

        closeMenus()

        if (!active) {
            var target = { target: this }
            $parent.trigger(e = $.Event('show.jn.dropdown', target))

            if (e.isDefaultPrevented()) return

            $this.trigger('focus')

            $parent
              .toggleClass('open')
              .trigger('shown.jn.dropdown', target)
        }
        return false
    }

    // Handles keys Up, Down and Escape
    ddl.prototype.keydown = function (e) {
        if (!/(27|38|40)/.test(e.keyCode)) return

        var $this = $(this)

        e.preventDefault()
        e.stopPropagation()

        if ($this.is('.disabled, :disabled')) return

        var $parent = getParent($this)
        var active = $parent.hasClass('open')

        // Escape - Close
        if (!active || (active && e.keyCode == 27)) {
            if (e.which == 27) $parent.find(dataToggle).trigger('focus')
            return $this.trigger('click')
        }

        var desc = ' li:not(.divider):visible a'
        var $items = $parent.find('[role="menu"]' + desc + ', [role="listbox"]' + desc)

        if (!$items.length) return

        var index = $items.index($items.filter(':focus'))

        // 38 - Move Up, 40 - Move down
        if (e.keyCode == 38 && index > 0) index--
        if (e.keyCode == 40 && index < $items.length - 1) index++

        if (!~index) index = 0

        $items.eq(index).trigger('focus')
    }

    function closeMenus(e) {
        if (e && e.which === 3) return
        $(dataToggle).each(function () {
            var $parent = getParent($(this))
            var target = { target: this }
            if (!$parent.hasClass('open')) return
            $parent.trigger(e = $.Event('hide.jn.dropdown', target))
            if (e.isDefaultPrevented()) return
            $parent.removeClass('open').trigger('hidden.jn.dropdown', target)
        })
    }

    function getParent($this) {
        var selector = $this.attr('data-target')

        if (!selector) {
            selector = $this.attr('href')
            selector = selector && /#[A-Za-z]/.test(selector) && selector.replace(/.*(?=#[^\s]*$)/, '') // strip for ie7
        }
        var $parent = selector && $(selector)

        return $parent && $parent.length ? $parent : $this.parent()
    }

    function plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('jn.dropdown')

            if (!data) $this.data('jn.dropdown', (data = new ddl(this)))
            if (typeof option == 'string') data[option].call($this)
        })
    }

    function changeValue($this) {
        var $button = $this.parents('ul').prev();
        $button.html($this.html() + '<span class="caret"></span>');
    }

    function clearFocus($this) {
        $this.eq(0).trigger('focus')
    }

    var old = $.fn.ddl
    $.fn.ddl = plugin
    $.fn.ddl.Constructor = ddl
    $.fn.ddl.noConflict = function () {
        $.fn.ddl = old
        return this
    }

    $(document)
      .on('click.jn.dropdown.data-api', closeMenus)
      .on('click.jn.dropdown.data-api', '.dropdown form', function (e) { e.stopPropagation() })
      .on('click.jn.dropdown.data-api', dataToggle, ddl.prototype.toggle)
      .on('keydown.jn.dropdown.data-api', dataToggle + ', [role="menu"], [role="listbox"]', ddl.prototype.keydown)

    $change.click(function () {
        changeValue($(this))
    });

    $change.mouseover(function () {
        clearFocus($(this))
    });

}(jQuery);


