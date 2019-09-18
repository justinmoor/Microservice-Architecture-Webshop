import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'eurcurrency'
})
export class EurcurrencyPipe implements PipeTransform {

  transform(value: number): string {
    if (!value){
      return "null";
    }
    if(value % 1 == 0){
      return "\u20AC " + value + ",-"
    }
    value = (Math.round(value * 100) / 100);
    return "\u20AC " + value.toLocaleString('nl-NL', { minimumFractionDigits: 2 })   
  }
}
