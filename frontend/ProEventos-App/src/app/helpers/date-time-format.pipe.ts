import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  public override transform(value: any, ...args: any[]): any {
    return super.transform(value, Constants.DATE_TIME_FMT);
  }

}
