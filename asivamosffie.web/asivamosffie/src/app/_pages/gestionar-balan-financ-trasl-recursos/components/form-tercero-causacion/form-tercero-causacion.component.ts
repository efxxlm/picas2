import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-tercero-causacion',
  templateUrl: './form-tercero-causacion.component.html',
  styleUrls: ['./form-tercero-causacion.component.scss']
})
export class FormTerceroCausacionComponent implements OnInit {

    @Input() solicitudPago: any;

    constructor() { }

    ngOnInit(): void {
    }

}
