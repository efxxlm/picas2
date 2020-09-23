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


@Component({
  selector: 'app-tabla-registrar-otros-temas',
  templateUrl: './tabla-registrar-otros-temas.component.html',
  styleUrls: ['./tabla-registrar-otros-temas.component.scss']
})
export class TablaRegistrarOtrosTemasComponent implements OnInit {

  @Input() objetoComiteTecnico: ComiteTecnico;
  @Input() esProposicionesVarios: boolean;
  @Output() validar: EventEmitter<any> = new EventEmitter();


  listaMiembros:Usuario[];

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

             ) 
  {}

  ngOnInit(): void {

    console.log( this.esProposicionesVarios )
    this.commonService.listaUsuarios().then(( respuesta )=>{
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

  openDialogValidacionSolicitudes( elemento: SesionComiteTema ) {

    elemento.sesionTemaVoto = [];

    console.log( this.objetoComiteTecnico.sesionParticipante.length )

    this.objetoComiteTecnico.sesionParticipante.forEach( p => {
      let votacion: SesionTemaVoto = p.sesionTemaVoto.find( v => v.sesionTemaId == elemento.sesionTemaId );
      let usuario: Usuario = this.listaMiembros.find( m => m.usuarioId == p.usuarioId ) 

      let temaVoto: SesionTemaVoto = {

        sesionTemaVotoId: votacion ? votacion.sesionTemaVotoId : 0,
        sesionTemaId: elemento.sesionTemaId,
        sesionParticipanteId: p.sesionParticipanteId,
        
        nombreParticipante: `${ usuario.nombres } ${ usuario.apellidos }`,
        esAprobado: votacion ? votacion.esAprobado : null,
        observacion: votacion ? votacion.observacion : null,

      }

      elemento.sesionTemaVoto.push( temaVoto )
    })


    const dialog = this.dialog.open(VotacionTemaComponent, {
      width: '70em', data: { sesionComiteTema: elemento }
    });

    dialog.afterClosed().subscribe( c => {
      if (c && c.comiteTecnicoId){
      this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId( c.comiteTecnicoId )
          .subscribe( response => {
            this.objetoComiteTecnico = response;
            this.validar.emit(null);
          })
        }
    })

  }

  changeRequiere( check: boolean, solicitudTema: SesionComiteTema ){
    
    this.objetoComiteTecnico.sesionComiteTema.forEach( tem => {
      if (tem.sesionTemaId == solicitudTema.sesionTemaId)
        if ( check ){
          tem.completo = false
        }else{
          tem.completo = true
          this.technicalCommitteSessionService.noRequiereVotacionSesionComiteTema( solicitudTema )
            .subscribe( respuesta => {
              
            })
        }
    })
  }

  cargarRegistro(){

    let lista = this.objetoComiteTecnico.sesionComiteTema.
                        filter( t => (t.esProposicionesVarios ? t.esProposicionesVarios : false) == this.esProposicionesVarios )

    this.dataSource = new MatTableDataSource( lista );

  }

}
