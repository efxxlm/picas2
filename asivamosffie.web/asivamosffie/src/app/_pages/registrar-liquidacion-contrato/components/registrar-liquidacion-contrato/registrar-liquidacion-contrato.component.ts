import { CommonService } from 'src/app/core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { LiquidacionContratoService } from 'src/app/core/_services/liquidacionContrato/liquidacion-contrato.service';
import { EstadoLiquidacionCodigo } from 'src/app/_interfaces/estados-liquidacion-contratos.interface';

@Component({
  selector: 'app-registrar-liquidacion-contrato',
  templateUrl: './registrar-liquidacion-contrato.component.html',
  styleUrls: ['./registrar-liquidacion-contrato.component.scss']
})
export class RegistrarLiquidacionContratoComponent implements OnInit {

    verAyuda = false;
    estadoLiquidacionCodigo = EstadoLiquidacionCodigo;
    listaAcordeonSinRegistro: any[] = [];
    listaAcordeonEnProcesoFirma: any[] = [];
    listaAcordeonLiquidado: any[] = [];
    semaforoSinRegistro = 'sin-diligenciar';
    semaforoEnProceso = 'sin-diligenciar';
    semaforoLiquidacion = 'completo';

    constructor(
        private commonSvc: CommonService,
        private liquidacionContratoSvc: LiquidacionContratoService )
    {
        this.liquidacionContratoSvc.getListContractSettlemen()
            .subscribe( getListContractSettlemen => {
                console.log( getListContractSettlemen );
                this.listaAcordeonSinRegistro = getListContractSettlemen[ 0 ];
                this.listaAcordeonEnProcesoFirma = getListContractSettlemen[ 1 ];
                this.listaAcordeonLiquidado = getListContractSettlemen[ 2 ];
                // Get  semaforo sin registro de liquidacion
                if ( this.listaAcordeonSinRegistro.length === 0 ) {
                    this.semaforoSinRegistro = 'completo';
                }
                // Get semaforo en proceso de firmas
                let enProceso = 0;
                this.listaAcordeonEnProcesoFirma.forEach( registro => {
                    if ( registro.estadoSolicitudCodigo === this.estadoLiquidacionCodigo.enProcesoDeFirmas || registro.estadoSolicitudCodigo === this.estadoLiquidacionCodigo.enProcesoFirmasFiduciaria ) {
                        enProceso++;
                    }
                } )
                if ( enProceso > 0 && ( enProceso < this.listaAcordeonEnProcesoFirma.length || enProceso === this.listaAcordeonEnProcesoFirma.length ) ) {
                    this.semaforoEnProceso = 'en-proceso';
                }
                if ( enProceso === 0 && ( this.listaAcordeonEnProcesoFirma.length === 0 || this.listaAcordeonEnProcesoFirma.length > 0 ) ) {
                    this.semaforoEnProceso = 'completo';
                }
            } );
        
    }

    ngOnInit(): void {
    }

}
