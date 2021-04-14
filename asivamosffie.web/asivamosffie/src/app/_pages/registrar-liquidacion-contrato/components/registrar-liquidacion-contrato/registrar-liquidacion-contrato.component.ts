import { CommonService } from 'src/app/core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { LiquidacionContratoService } from 'src/app/core/_services/liquidacionContrato/liquidacion-contrato.service';

@Component({
  selector: 'app-registrar-liquidacion-contrato',
  templateUrl: './registrar-liquidacion-contrato.component.html',
  styleUrls: ['./registrar-liquidacion-contrato.component.scss']
})
export class RegistrarLiquidacionContratoComponent implements OnInit {

    verAyuda = false;
    listaAcordeonSinRegistro: any[] = [];
    listaAcordeonEnProcesoFirma: any[] = [];
    listaAcordeonLiquidado: any[] = [];

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
            } );
        
    }

    ngOnInit(): void {
    }

}
