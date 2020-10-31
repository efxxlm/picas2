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
    'responsable',
    'fechaCumplimiento',
    'fechaReporte',
    'estadoReportado',
    'gestionRealizada',
    'id'
  ];
  dataSource = new MatTableDataSource();

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
    private commonService: CommonService,

  ) { }

  ngOnInit(): void {


    this.estadosArray

    this.activatedRoute.params.subscribe(parametros => {

      forkJoin([
        this.technicalCommitteeSessionService.getCompromisosByComiteTecnicoId(parametros.id),
        this.commonService.listaEstadoCompromisos(),

      ]).subscribe(respuesta => {
        this.comite = respuesta[0];
        respuesta[0].sesionComiteTema.forEach(tem => {
          tem.temaCompromiso.forEach(tc => {
            tc.nombreResponsable = `${tc.responsableNavigation.usuario.nombres} ${tc.responsableNavigation.usuario.apellidos}`;
            tc.nombreEstado = tc.estadoCodigo;
            tc.estadoCodigo = null;
          });
          this.listaCompromisos = this.listaCompromisos.concat(tem.temaCompromiso);
        })

        if (respuesta[0].sesionComiteSolicitudComiteTecnico) {
          respuesta[0].sesionComiteSolicitudComiteTecnico.forEach(sol => {
            sol.sesionSolicitudCompromiso.forEach(sc => {
              sc.nombreResponsable = `${sc.responsableSesionParticipante.usuario.nombres} ${sc.responsableSesionParticipante.usuario.apellidos}`
              sc.nombreEstado = sc.estadoCodigo;
              sc.estadoCodigo = null;
            });
            this.listaCompromisos = this.listaCompromisos.concat(sol.sesionSolicitudCompromiso);
          })
        }
        console.log( respuesta[0] );
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

  onChange(id: number, valor: any) {

  }

  openVerDetalle(id: number) {
    this.dialog.open(DialogVerDetalleComponent, {
      width: '70em'
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSave() {

    let comite: ComiteTecnico = {
      sesionComiteTema: [],
      sesionComiteSolicitudComiteTecnico: []
    }

    let tema: SesionComiteTema = {
      temaCompromiso: this.listaCompromisos.filter(c => c.temaCompromisoId > 0 && c.estadoCodigo)
    }
    if (tema.temaCompromiso.length > 0)
      comite.sesionComiteTema.push(tema);

    let solicitud: SesionComiteSolicitud = {
      sesionSolicitudCompromiso: this.listaCompromisos.filter(c => c.sesionSolicitudCompromisoId > 0 && c.estadoCodigo)
    }
    if (solicitud.sesionSolicitudCompromiso.length > 0)
      comite.sesionComiteSolicitudComiteTecnico.push(solicitud);

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
