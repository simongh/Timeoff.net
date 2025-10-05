import { Component, input, ViewEncapsulation } from '@angular/core';

import { SvgIcon } from './svg';

@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: 'svg[calendar-week]',
  template: `<svg:path stroke="none" d="M0 0h24v24H0z" fill="none"/>
  <svg:path d="M4 7a2 2 0 0 1 2 -2h12a2 2 0 0 1 2 2v12a2 2 0 0 1 -2 2h-12a2 2 0 0 1 -2 -2v-12z" />
  <svg:path d="M16 3v4" />
  <svg:path d="M8 3v4" />
  <svg:path d="M4 11h16" />
  <svg:path d="M7 14h.013" />
  <svg:path d="M10.01 14h.005" />
  <svg:path d="M13.01 14h.005" />
  <svg:path d="M16.015 14h.005" />
  <svg:path d="M13.015 17h.005" />
  <svg:path d="M7.01 17h.005" />
  <svg:path d="M10.01 17h.005" />`,
  host: {
    '[attr.xmlns]': 'xmlns',
    '[attr.width]': 'width()',
    '[attr.height]': 'height()',
    '[attr.viewBox]': 'viewBox()',
    '[attr.fill]': 'fill()',
    stroke: 'currentColor',
    'stroke-width': '2',
    'stroke-linecap': 'round',
    'stroke-linejoin': 'round',
    class: 'icon icon-tabler icons-tabler-outline icon-calendar-week',
  },
  encapsulation: ViewEncapsulation.None,
})
export class CalendarWeekIcon extends SvgIcon {
}