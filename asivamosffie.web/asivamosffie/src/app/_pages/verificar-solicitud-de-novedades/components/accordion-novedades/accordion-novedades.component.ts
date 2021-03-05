import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-accordion-novedades',
  templateUrl: './accordion-novedades.component.html',
  styleUrls: ['./accordion-novedades.component.scss']
})
export class AccordionNovedadesComponent implements OnInit {
  @Input() tiposNovedadModificacionContractual;
  constructor() { }

  ngOnInit(): void {
  }

}
