import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-informe-final-rlc',
  templateUrl: './tabla-informe-final-rlc.component.html',
  styleUrls: ['./tabla-informe-final-rlc.component.scss']
})
export class TablaInformeFinalRlcComponent implements OnInit {

    @Input() contrato: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaEnvioInformeFinal',
      'fechaAprobacionInformeFinal',
      'llaveMen',
      'tipoIntervencion',
      'institucionEducativa',
      'sede',
      'gestion'
    ];
    listaMenu = ListaMenuSolicitudLiquidacionId;
    datosTabla = [];

    constructor(
        private router: Router,
        private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService )
    { }

    async ngOnInit() {
            console.log(this.contrato);
            const proyecto = await this.registerContractualLiquidationRequestService.gridInformeFinal( this.contrato?.contratacionId, this.listaMenu.registrarSolicitudLiquidacionContratacion ).toPromise();

            if ( proyecto !== null ) {
                proyecto.forEach( registro => {
                    this.datosTabla.push({
                        fechaEnvio : registro.fechaEnvio !== undefined ? registro.fechaEnvio.split('T')[0].split('-').reverse().join('/') : '',
                        fechaAprobacion : registro.fechaAprobacion !== undefined ? registro.fechaAprobacion.split('T')[0].split('-').reverse().join('/') : '',
                        llaveMen: registro.llaveMen,
                        tipoIntervencion: registro.tipoIntervencion,
                        institucionEducativa: registro.institucionEducativa,
                        sede: registro.sede,
                        estadoValidacion: registro.registroCompleto ? 'Con validación' : 'Sin validación',
                        registroCompleto: registro.registroCompleto ? 'Completo' : 'Incompleto',
                        contratacionId: this.contrato?.contratacionId,
                        proyectoId: registro.proyectoId
                    });
                } )
            }

        this.dataSource = new MatTableDataSource( this.datosTabla );
    }

}
