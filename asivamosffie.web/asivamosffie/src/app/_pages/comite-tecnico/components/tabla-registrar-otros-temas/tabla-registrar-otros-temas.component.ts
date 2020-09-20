import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { ComiteTecnico, SesionComiteTema, SesionTemaVoto } from 'src/app/_interfaces/technicalCommitteSession';
import { VotacionTemaComponent } from '../votacion-tema/votacion-tema.component';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { TagContentType } from '@angular/compiler';


@Component({
  selector: 'app-tabla-registrar-otros-temas',
  templateUrl: './tabla-registrar-otros-temas.component.html',
  styleUrls: ['./tabla-registrar-otros-temas.component.scss']
})
export class TablaRegistrarOtrosTemasComponent implements OnInit {

  @Input() objetoComiteTecnico: ComiteTecnico;
  @Input() esProposicionesVarios: boolean;
  @Output() validar: EventEmitter<any> = new EventEmitter();


  listaMiembros: Usuario[];

  displayedColumns: string[] = ['responsable', 'tiempo', 'tema', 'votacion', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    public dialog: MatDialog,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    private commonService: CommonService,

  ) { }

  ngOnInit(): void {

    console.log(this.esProposicionesVarios)
    this.commonService.listaUsuarios().then((respuesta) => {
      this.listaMiembros = respuesta;
    })

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

  openDialogValidacionSolicitudes(elemento: SesionComiteTema) {

    let sesionComiteTema: SesionComiteTema = {
      sesionTemaId: elemento.sesionTemaId,
      sesionTemaVoto: [],
      
    }

    console.log(this.objetoComiteTecnico.sesionParticipante.length)

    this.objetoComiteTecnico.sesionParticipante.forEach(p => {
      let temaVoto: SesionTemaVoto = elemento.sesionTemaVoto.find(v => v.sesionTemaId == elemento.sesionTemaId && v.sesionParticipanteId == p.sesionParticipanteId);
      let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)

      if ( temaVoto ){
        temaVoto.nombreParticipante = `${usuario.nombres} ${usuario.apellidos}`;
      }else{
        temaVoto = {

          sesionTemaVotoId: 0,
          sesionTemaId: elemento.sesionTemaId,
          sesionParticipanteId: p.sesionParticipanteId,
          esAprobado: null,
          observacion: null,
  
        }  
      }

      

      

      sesionComiteTema.sesionTemaVoto.push(temaVoto)
    })


    const dialog = this.dialog.open(VotacionTemaComponent, {
      width: '70em', data: { sesionComiteTema: sesionComiteTema }
    });

    dialog.afterClosed().subscribe(c => {
      // if (c && c.comiteTecnicoId) {
      //   this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
      //     .subscribe(response => {
      //       this.objetoComiteTecnico = response;
      this.validar.emit(null);
      //     })
      // }
    })

  }

  changeRequiere(check: boolean, solicitudTema: SesionComiteTema) {

    this.objetoComiteTecnico.sesionComiteTema.forEach(tem => {
      if (tem.sesionTemaId == solicitudTema.sesionTemaId) {
        this.technicalCommitteSessionService.noRequiereVotacionSesionComiteTema(solicitudTema, check)
          .subscribe(respuesta => {
            tem.completo = !check
            this.validar.emit(null);
          })
      }
    });

  }

  validarResgistros( listaSesionComiteTema: SesionComiteTema[] ){
    listaSesionComiteTema.forEach( ct => {
      ct.completo = true;

      if ( ct.requiereVotacion == true && ct.sesionTemaVoto.length == 0 ) { ct.completo = false }

      ct.sesionTemaVoto.forEach( tv => {
        if ( tv.esAprobado != true && tv.esAprobado != false ){
          ct.completo = false;
        }
      })
    })
  }

  cargarRegistro() {

    let lista = this.objetoComiteTecnico.sesionComiteTema.
      filter(t => (t.esProposicionesVarios ? t.esProposicionesVarios : false) == this.esProposicionesVarios)

    this.validarResgistros( lista )

    this.dataSource = new MatTableDataSource(lista);

  }

}
