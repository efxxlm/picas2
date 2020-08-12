import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-asociada',
  templateUrl: './asociada.component.html',
  styleUrls: ['./asociada.component.scss']
})
export class AsociadaComponent {

  solicitudAsociada: FormControl;
  solicitudAsociadaArray = [
    {name: 'Obra', value: '1'},
    {name: 'Interventoría', value: '2'}
  ];


  constructor( ) {
    this.declararInput();
  }

  private declararInput() {
    this.solicitudAsociada = new FormControl('', [Validators.required]);
  }

}
