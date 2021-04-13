import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DialogVerDetalleComponent } from '../dialog-ver-detalle/dialog-ver-detalle.component';
import { ActivatedRoute } from '@angular/router';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ComiteTecnico, SesionComiteTema, SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionSecretarioComponent } from '../observacion-secretario/observacion-secretario.component';
import { componentFactoryName } from '@angular/compiler';


@Component({
  selector: 'app-tabla-verificar-cumplimiento',
  templateUrl: './tabla-verificar-cumplimiento.component.html',
  styleUrls: ['./tabla-verificar-cumplimiento.component.scss']
})
export class TablaVerificarCumplimientoComponent implements OnInit {

  listaCompromisos: any[] = [];
  comite: any;
  displayedColumns: string[] = [
    'tarea',
    'nombreResponsable',
    'fechaCumplimiento',
    'fechaModificacion',
    'nombreEstado',
    'gestionRealizada',
    'id'
  ];
  dataSource = new MatTableDataSource();
  estaCumplido: any[] = [ 'Cumplido', 'No Cumplido' ];

  estadosArray: Dominio[] = []

  estaEditando = false;

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
    private commonService: CommonService,

  ) { }

  ngOnInit(): void {


    this.estadosArray

    this.activatedRoute.params.subscribe(parametros => {

      forkJoin([
        this.technicalCommitteeSessionService.getCompromisosByComiteTecnicoId(parametros.id, false),
        this.commonService.listaEstadoCompromisos(),

      ]).subscribe(respuesta => {
        this.comite = respuesta[0];
        respuesta[0].sesionComiteTema.forEach(tem => {
          tem.temaCompromiso.forEach(tc => {
            tc.nombreResponsable = `${tc.responsableNavigation.usuario.primerNombre} ${tc.responsableNavigation.usuario.primerApellido}`;
            tc.nombreEstado = tc.estadoCodigo;
            tc.estadoCodigo = null;
            tc['temaCompromisoSeguimiento'] = tc['temaCompromisoSeguimiento'];
            tc[ 'tieneCompromisos' ] = tc['temaCompromisoSeguimiento'].length > 0 ? true : false;
            tc[ 'compromisoSeleccionado' ] = tc[ 'esCumplido' ] !== undefined ? ( tc[ 'esCumplido' ] === true ? 'Cumplido' : 'No Cumplido' ) : null;
            tc[ 'esCumplido' ] = tc[ 'esCumplido' ] !== undefined ? tc[ 'esCumplido' ] : null;
          });
          this.listaCompromisos = this.listaCompromisos.concat(tem.temaCompromiso);
        })

        if (respuesta[0].sesionComiteSolicitudComiteTecnico) {
          respuesta[0].sesionComiteSolicitudComiteTecnico.forEach(sol => {
            sol.sesionSolicitudCompromiso.forEach(sc => {
              sc.nombreResponsable = `${sc.responsableSesionParticipante.usuario.primerNombre} ${sc.responsableSesionParticipante.usuario.primerApellido}`
              sc.nombreEstado = sc.estadoCodigo;
              sc.estadoCodigo = null;
              sc[ 'tieneCompromisos' ] = sc[ 'compromisoSeguimiento' ].length > 0 ? true : false;
              sc[ 'compromisoSeleccionado' ] = sc[ 'esCumplido' ] !== undefined ? ( sc[ 'esCumplido' ] === true ? 'Cumplido' : 'No Cumplido' ) : null
              sc[ 'esCumplido' ] = sc[ 'esCumplido' ] !== undefined ? sc[ 'esCumplido' ] : null;
            });
            this.listaCompromisos = this.listaCompromisos.concat(sol.sesionSolicitudCompromiso);
          })
        }
        console.log( respuesta[0], this.listaCompromisos );
        this.estadosArray = respuesta[1];
        this.listaCompromisos.forEach(element => {
          element.fechaCumplimiento = element.fechaCumplimiento
            ? element.fechaCumplimiento.split('T')[0].split('-').reverse().join('/')
            : '';
          element.fechaModificacion =  element.fechaModificacion
            ? element.fechaModificacion.split('T')[0].split('-').reverse().join('/')
            : '';
          element.fechaCreacion =  element.fechaCreacion
            ? element.fechaCreacion.split('T')[0].split('-').reverse().join('/')
            : '';
        });
        this.dataSource = new MatTableDataSource(this.listaCompromisos);
        this.initPaginator();

      })
    })
  }

  initPaginator() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
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
    this.paginator._intl.previousPageLabel = 'Anterior';
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
    this.estaEditando = true;
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
      sesionComiteSolicitudComiteTecnico: [
        {
          sesionSolicitudCompromiso: []
        }
      ]
    };

    this.listaCompromisos.forEach( compromiso => {
      if ( compromiso.sesionSolicitudCompromisoId !== undefined ) {
        compromiso.esCumplido = compromiso.compromisoSeleccionado == 'Cumplido' ? true : false; 
        //compromiso.responsableSesionParticipante = null;
        //compromiso.compromisoSeguimiento = null;
        compromiso.fechaCreacion = new Date();
        compromiso.fechaCumplimiento = new Date();
        compromiso.fechaModificacion = new Date();
        comite.sesionComiteSolicitudComiteTecnico[0].sesionSolicitudCompromiso.push( compromiso );
        console.log( compromiso )
        
      };
      if ( compromiso.temaCompromisoId !== undefined ) {
        compromiso.esCumplido = compromiso.compromisoSeleccionado == 'Cumplido' ? true : false;
        comite.sesionComiteTema[0].temaCompromiso.push( compromiso );
        console.log( compromiso )
      };
    } );

    console.log( comite );
    this.technicalCommitteeSessionService.verificarTemasCompromisos(comite)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        if (respuesta.code == "200") {
          this.listaCompromisos = [];
          this.ngOnInit();
        }
      })

  }

}
