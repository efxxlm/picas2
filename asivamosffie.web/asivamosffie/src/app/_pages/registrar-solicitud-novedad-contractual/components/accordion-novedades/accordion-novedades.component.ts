import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { NovedadContractual, NovedadContractualDescripcion } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-accordion-novedades',
  templateUrl: './accordion-novedades.component.html',
  styleUrls: ['./accordion-novedades.component.scss']
})
export class AccordionNovedadesComponent implements OnInit {
  @Input () tiposNovedadModificacionContractual;
  @Input () novedadeContractual: any;//NovedadContractual[];
  
  constructor() { }

  ngOnInit(): void {
  }

}
