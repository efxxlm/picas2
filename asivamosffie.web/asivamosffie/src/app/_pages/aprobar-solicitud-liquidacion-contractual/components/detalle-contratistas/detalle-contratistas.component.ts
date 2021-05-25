import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-contratistas',
  templateUrl: './detalle-contratistas.component.html',
  styleUrls: ['./detalle-contratistas.component.scss']
})
export class DetalleContratistasComponent implements OnInit {

  @Input() contratista: any;

  constructor() { }

  ngOnInit(): void {
  }

}
