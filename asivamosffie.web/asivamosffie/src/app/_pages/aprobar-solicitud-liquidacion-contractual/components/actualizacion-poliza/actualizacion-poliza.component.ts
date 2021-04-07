import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-actualizacion-poliza',
  templateUrl: './actualizacion-poliza.component.html',
  styleUrls: ['./actualizacion-poliza.component.scss']
})
export class ActualizacionPolizaComponent implements OnInit {

  contratacionProyectoId: number;
  contratoPolizaActualizacionId: number;//definir
  listaTipoObservacionLiquidacionContratacion: any;
  listaMenu: any;
  @Output() semaforoActualizacionPoliza = new EventEmitter<string>();

  listaPolizas = [
    {
      poliza: 'Buen manejo y correcta inversión del anticipo',
      responsable: 'Andres Nikolai Montealegre Rojas'
    },
    {
      poliza: 'Garantía de estabilidad y calidad de la obra',
      responsable: 'Andres Nikolai Montealegre Rojas'
    }
  ]

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
    });
   }

   ngOnInit(): void {
    this.registerContractualLiquidationRequestService.listaTipoObservacionLiquidacionContratacion()
    .subscribe( response => {
        this.listaTipoObservacionLiquidacionContratacion = response;
    });
    this.registerContractualLiquidationRequestService.listaMenu()
    .subscribe( response => {
        this.listaMenu = response;
    });
  }  

}
