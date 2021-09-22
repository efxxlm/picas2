import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-accordion-novedades',
  templateUrl: './accordion-novedades.component.html',
  styleUrls: ['./accordion-novedades.component.scss']
})
export class AccordionNovedadesComponent implements OnInit {
  @Input () tiposNovedadModificacionContractual;
  @Input () novedadeContractual: any;//NovedadContractual[];
  @Output() guardar = new EventEmitter();
  @Input() contrato: any;

  constructor() { }

  ngOnInit(): void {
  }

  guardarRegistro(){
    this.guardar.emit(true);
  }

}
