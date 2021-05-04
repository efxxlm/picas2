import { Component, Input, OnInit } from '@angular/core';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual, NovedadContractualObservaciones } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-solicitud-novedad',
  templateUrl: './solicitud-novedad.component.html',
  styleUrls: ['./solicitud-novedad.component.scss']
})
export class SolicitudNovedadComponent implements OnInit {

  @Input() novedad: NovedadContractual;

  tipoNovedadNombre: string = '';

  listaObservaciones = [
    {
      llaveMEN: 'LJ776554',
      departamento: 'Atlántico',
      municipio: 'Galapa',
      institucionEducativa: 'I.E. María Villa Campo',
      sede: 'Única sede'
    }
  ]

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,

  ) { }

  ngOnInit(): void {

    this.novedad.novedadContractualDescripcion.forEach(d => {
        this.tipoNovedadNombre = this.tipoNovedadNombre + d.nombreTipoNovedad + ', ' 
    });

    //this.contractualNoveltyService.get
  }

}
