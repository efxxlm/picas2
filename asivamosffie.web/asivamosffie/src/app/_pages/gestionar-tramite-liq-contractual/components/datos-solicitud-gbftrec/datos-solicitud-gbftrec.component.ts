import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-datos-solicitud-gbftrec',
  templateUrl: './datos-solicitud-gbftrec.component.html',
  styleUrls: ['./datos-solicitud-gbftrec.component.scss']
})
export class DatosSolicitudGbftrecComponent implements OnInit {

    @Input() solicitudPago: any;
    dataSource = new MatTableDataSource();
    listaTipoSolicitud = TipoSolicitudes;
    listaTipoSolicitudContrato: Dominio[] = [];
    valorTotalFactura = 0;
    solicitudPagoFase: any;
    displayedColumns: string[] = [
        'llaveMen',
        'tipoIntervencion',
        'departamento',
        'municipio',
        'institucionEducativa',
        'sede'
    ];
    dataTable: any[] = [{
      llaveMen: 'LL457326',
      tipoIntervencion: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Susacón',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen',
      sede: 'Única sede'
    }]

    constructor( private commonSvc: CommonService ) {
    }

    async ngOnInit() {
        this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise()
        if ( this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.expensas && this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.otrosCostos ) {
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];

            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => this.valorTotalFactura += criterio.valorFacturado );
        }

        this.dataSource = new MatTableDataSource( this.solicitudPago.contratoSon.listProyectos[1] );
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        /*
        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.otrosCostos ) {
            return 'Otros costos y servicios';
        }

        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.expensas ) {
            return 'Expensas';
        }
        */

        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }

}
