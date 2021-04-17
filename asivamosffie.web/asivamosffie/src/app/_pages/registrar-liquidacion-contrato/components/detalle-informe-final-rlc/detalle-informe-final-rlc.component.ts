import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Location } from '@angular/common';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-detalle-informe-final-rlc',
  templateUrl: './detalle-informe-final-rlc.component.html',
  styleUrls: ['./detalle-informe-final-rlc.component.scss']
})
export class DetalleInformeFinalRlcComponent implements OnInit {
    //Tabla informe final y anexos
    listaMenu = ListaMenuSolicitudLiquidacionId;
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    displayedColumns: string[] = ['numero','item','tipoAnexo','ubicacion'];
    data: any;
    datosTabla: any[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        private location: Location,
        private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService )
    {

        this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(
            this.activatedRoute.snapshot.params.id,
            this.activatedRoute.snapshot.params.idContratacionProyecto,
            this.listaMenu.registrarSolicitudLiquidacionContratacion )
            .subscribe(
                report => {
                    if( report != null ){
                        this.data = report[0];
                    }
                }
            );
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }

}
