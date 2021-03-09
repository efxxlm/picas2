import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectContractingService } from '../../../../core/_services/projectContracting/project-contracting.service';
import { CommonService } from '../../../../core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionDialogComponent } from '../observacion-dialog/observacion-dialog.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';
import { Contratacion } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-tabla-decisiones-acta',
  templateUrl: './tabla-decisiones-acta.component.html',
  styleUrls: ['./tabla-decisiones-acta.component.scss']
})
export class TablaDecisionesActaComponent implements OnInit {

  @Input() contratacionId: number;
  @Input() tipoSolicitudCodigo: string;
  dataSource = new MatTableDataSource();
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  data: any[] = [];
  listaEstadoProyectos: any[] = [];
  displayedColumns: string[] = [ 'llaveMen', 'tipoIntervencion', 'departamentoMunicipio', 'institucionEducativa', 'sede', 'estadoProyecto', 'gestion' ];
  contratacion: Contratacion;
  listaTipoSolicitudCodigo = {
    obra: '1',
    interventoria: '2'
  };

  constructor ( private projectContractingSvc: ProjectContractingService,
                private commonSvc: CommonService,
                private compromisoActaSvc: CompromisosActasComiteService,
                private dialog: MatDialog ) {
  }

  ngOnInit(): void {
    this.commonSvc.listaEstadoProyecto()
      .subscribe( ( resp: any ) => this.listaEstadoProyectos = resp );

    if ( this.contratacionId !== null && this.tipoSolicitudCodigo !== null ) {

      this.commonSvc.listaTipoSolicitud()
      .subscribe( ( response: any[] ) => {

        const tipoSolicitud = response.filter( tipo => tipo.codigo === this.tipoSolicitudCodigo );

        if ( tipoSolicitud[0].nombre === 'Contratación' ) {
          this.projectContractingSvc.getContratacionByContratacionIdWithGrillaProyecto( this.contratacionId )
          .subscribe( ( resp: any ) => {
            this.contratacion = resp;
            for ( let contratacion of resp.contratacionProyecto ) {
              console.log( contratacion.contratacionObservacion );
              this.data.push( { contratacion: contratacion.proyectoGrilla, sesionSolicitudObservacionProyecto: contratacion.contratacionObservacion, tipoSolicitudCodigo: resp.tipoSolicitudCodigo } )
            }
            this.dataSource = new MatTableDataSource( this.data );
            this.dataSource.sort = this.sort;
          } )
        }
        
      } )

    }
  }

  estadoObservaciones ( estadoProyecto: string ) {
    if ( this.listaEstadoProyectos.length !== 0 ) {
      if ( estadoProyecto === this.listaEstadoProyectos[4].codigo || estadoProyecto === this.listaEstadoProyectos[6].codigo ) {
        return false;
      } else {
        return true;
      };
    };
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '50em',
      data : { modalTitle, modalText }
    });
  };

  openDialogObservacion ( elemento: any ) {
    if ( elemento.sesionSolicitudObservacionProyecto.length === 0 ) {
      this.openDialog( `El proyecto en estado ${ elemento.contratacion.estadoProyecto }`, 'No contiene observaciones' )
      return;
    } else {
      this.dialog.open( ObservacionDialogComponent, {
        width: '80em',
        data : { elemento }
      });
    }
  };

}
