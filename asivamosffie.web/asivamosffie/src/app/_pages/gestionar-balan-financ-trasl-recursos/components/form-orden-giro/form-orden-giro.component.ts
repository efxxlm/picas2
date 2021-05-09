import { Component, OnInit, OnChanges, SimpleChanges, Input } from '@angular/core';

@Component({
  selector: 'app-form-orden-giro',
  templateUrl: './form-orden-giro.component.html',
  styleUrls: ['./form-orden-giro.component.scss']
})
export class FormOrdenGiroComponent implements OnInit, OnChanges {

    @Input() listaBusqueda = [];

    constructor() { }

    ngOnChanges( changes: SimpleChanges ): void {
        console.log( changes.listaBusqueda.currentValue )
    }

    ngOnInit(): void {
    }

}
