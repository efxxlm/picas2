import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-tabla-detalle-informe-final',
  templateUrl: './tabla-detalle-informe-final.component.html',
  styleUrls: ['./tabla-detalle-informe-final.component.scss']
})
export class TablaDetalleInformeFinalComponent implements OnInit {

    @Input() informeFinalId: number;
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    displayedColumns: string[] = ['numero','item','tipoAnexo','ubicacion'];

    constructor( private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService ) { }

    ngOnInit(): void {
        this.registerContractualLiquidationRequestService.getInformeFinalAnexoByInformeFinalId( this.informeFinalId )
        .subscribe(
            report => {
                const datosTabla = [];

                if( report != null ){
                    report.forEach(element => {
                        datosTabla.push({
                            numero: element.informeFinalListaChequeo.orden,
                            item: element.informeFinalListaChequeo.listaChequeoItem.nombre,
                            tipoAnexoString: element.informeFinalAnexo ? element.informeFinalAnexo.tipoAnexoString : "",
                            calificacionCodigo: element.calificacionCodigo,
                            numRadicadoSac: element.informeFinalAnexo ? element.informeFinalAnexo.numRadicadoSac : "",
                            fechaRadicado:element.informeFinalAnexo ? element.informeFinalAnexo.fechaRadicado : "",
                            urlSoporte:  element.informeFinalAnexo ?element.informeFinalAnexo.urlSoporte : "",
                            tipoAnexo: element.informeFinalAnexo ? element.informeFinalAnexo.tipoAnexo : ""
                        });
                    })
                }

                this.dataSource = new MatTableDataSource( datosTabla );
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
                this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
            }
        );
    }

}
