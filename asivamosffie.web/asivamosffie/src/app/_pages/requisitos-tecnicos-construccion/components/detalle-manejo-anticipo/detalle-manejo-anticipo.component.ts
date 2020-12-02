import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-detalle-manejo-anticipo',
  templateUrl: './detalle-manejo-anticipo.component.html',
  styleUrls: ['./detalle-manejo-anticipo.component.scss']
})
export class DetalleManejoAnticipoComponent implements OnInit {

  @Input() contratacion: any;

  constructor() { }


  ngOnInit(): void {
  }

}
