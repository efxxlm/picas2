import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComiteTecnico, SesionComiteSolicitud, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';
import { DialogVerDetalleComponent } from '../dialog-ver-detalle/dialog-ver-detalle.component';
import { ObservacionSecretarioComponent } from '../observacion-secretario/observacion-secretario.component';
import { VerDetallesComponent } from '../ver-detalles/ver-detalles.component';

@Component({
  selector: 'app-tabla-verificar-cumplimientos',
  templateUrl: './tabla-verificar-cumplimientos.component.html',
  styleUrls: ['./tabla-verificar-cumplimientos.component.scss']
})
export class TablaVerificarCumplimientosComponent implements OnInit {

  listaCompromisos: any[] = [];
  comite: any;
  displayedColumns: string[] = [
    'tarea',
    'responsable',
    'fechaCumplimiento',
    'fechaReporte',
    'estadoReportado',
    'gestionRealizada',
    'id'
  ];
  dataSource = new MatTableDataSource();
  estaCumplido: any[] = [ 'Cumplido', 'No Cumplido' ];
  estadosArray: Dominio[] = []

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private commonService: CommonService,

  ) { }

  ngOnInit(): void {


    this.estadosArray

    this.activatedRoute.params.subscribe(parametros => {

      forkJoin([
        this.technicalCommitteeSessionService.getCompromisosByComiteTecnicoId(parametros.id),
        this.commonService.listaEstadoCompromisos()
      ]).subscribe(respuesta => {
        console.log( respuesta[0] );
        this.comite = respuesta[0];
        respuesta[0].sesionComiteTema.forEach(tem => {
          tem.temaCompromiso.forEach(tc => {
            tc.nombreResponsable = `${tc.responsableNavigation.usuario.nombres} ${tc.responsableNavigation.usuario.apellidos}`;
            tc.nombreEstado = tc.estadoCodigo;
            tc.estadoCodigo = null;
            tc[ 'temaCompromisoSeguimiento' ] = tc[ 'temaCompromisoSeguimiento' ];
            tc[ 'tieneCompromisos' ] = tc['temaCompromisoSeguimiento'].length > 0 ? true : false;
            tc[ 'compromisoSeleccionado' ] = tc[ 'esCumplido' ] !== undefined ? ( tc[ 'esCumplido' ] === true ? 'Cumplido' : 'No Cumplido' ) : null;
            tc[ 'esCumplido' ] = tc[ 'esCumplido' ] !== undefined ? tc[ 'esCumplido' ] : null;
          });
          this.listaCompromisos = this.listaCompromisos.concat(tem.temaCompromiso);
        })

        if (respuesta[0].sesionComiteSolicitudComiteTecnicoFiduciario) {
          respuesta[0].sesionComiteSolicitudComiteTecnicoFiduciario.forEach(sol => {
            sol.sesionSolicitudCompromiso.forEach(sc => {
              sc.nombreResponsable = `${sc.responsableSesionParticipante.usuario.nombres} ${sc.responsableSesionParticipante.usuario.apellidos}`
              sc.nombreEstado = sc.estadoCodigo;
              sc.estadoCodigo = null;
              sc[ 'tieneCompromisos' ] = sc[ 'compromisoSeguimiento' ].length > 0 ? true : false;
              sc[ 'compromisoSeleccionado' ] = sc[ 'esCumplido' ] !== undefined ? ( sc[ 'esCumplido' ] === true ? 'Cumplido' : 'No Cumplido' ) : null
              sc[ 'esCumplido' ] = sc[ 'esCumplido' ] !== undefined ? sc[ 'esCumplido' ] : null;
            });
            this.listaCompromisos = this.listaCompromisos.concat(sol.sesionSolicitudCompromiso);
          })
        }
        console.log( this.listaCompromisos );
        this.estadosArray = respuesta[1];
        this.dataSource = new MatTableDataSource(this.listaCompromisos);
        this.initPaginator();

      })
    })
  }

  initPaginator() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  onChange ( compromiso: any, compromisoSeleccionado: string ) {
    this.listaCompromisos.forEach( value => {
      if ( value.sesionSolicitudCompromisoId === compromiso.sesionSolicitudCompromisoId ) {
        value[ 'esCumplido' ] = !compromisoSeleccionado.includes( 'No' );
      };
    } );
    if ( ( compromiso.nombreEstado === 'En proceso'  || compromiso.nombreEstado === 'Finalizado' ) && compromisoSeleccionado === 'No Cumplido' ) {
      this.openObservacionSecretario( compromiso );
    };
  };

  openVerDetalle( compromisoSeguimiento: any[] ) {
    this.dialog.open(DialogVerDetalleComponent, {
      width: '70em',
      data: { compromisos: compromisoSeguimiento }
    });
  };

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  openObservacionSecretario( compromisoSeguimiento: any[] ) {
    this.dialog.open( ObservacionSecretarioComponent, {
      width: '70em',
      data: { compromisos: compromisoSeguimiento }
    });
  };

  onSave() {

    const compromisosIncompletos = this.listaCompromisos.filter( value => value[ 'esCumplido' ] === null );
    console.log( compromisosIncompletos, this.listaCompromisos );
    if ( compromisosIncompletos.length > 0 ) {
      this.openDialog( '', '<b>Falta registrar información</b>' )
      return;
    };

    let comite: ComiteTecnico = {
      sesionComiteTema: [
        {
          temaCompromiso: []
        }
      ],
      sesionComiteSolicitudComiteTecnicoFiduciario: [
        {
          sesionSolicitudCompromiso: []
        }
      ]
    };

    this.listaCompromisos.forEach( compromiso => {
      if ( compromiso.sesionSolicitudCompromisoId !== undefined ) {
        comite.sesionComiteSolicitudComiteTecnicoFiduciario[0].sesionSolicitudCompromiso.push( compromiso );
      };
      if ( compromiso.temaCompromisoId !== undefined ) {
        comite.sesionComiteTema[0].temaCompromiso.push( compromiso );
      };
    } );

    this.technicalCommitteeSessionService.verificarTemasCompromisos(comite)
      .subscribe(
        respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`)
          if (respuesta.code == "200") {
            this.listaCompromisos = [];
            this.ngOnInit();
          };
        },
        err => this.openDialog( '', err.message )
      );

  };

}
