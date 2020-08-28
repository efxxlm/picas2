import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { ComiteTecnico, SesionComiteSolicitud, SesionSolicitudVoto } from 'src/app/_interfaces/technicalCommitteSession';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-tabla-registrar-validacion-solicitudes-contractiales',
  templateUrl: './tabla-registrar-validacion-solicitudes-contractiales.component.html',
  styleUrls: ['./tabla-registrar-validacion-solicitudes-contractiales.component.scss']
})
export class TablaRegistrarValidacionSolicitudesContractialesComponent implements OnInit {

  @Input() ObjetoComiteTecnico: ComiteTecnico;
  listaMiembros:Usuario[];

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'votacion', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
              public dialog: MatDialog,
              private commonService: CommonService,

             ) 
  {

  }

  openDialogValidacionSolicitudes( elemento: SesionComiteSolicitud ) {


    console.log(elemento, this.ObjetoComiteTecnico)

    elemento.sesionSolicitudVoto = [];

    this.ObjetoComiteTecnico.sesionParticipante.forEach( p => {
      let votacion: SesionSolicitudVoto = p.sesionSolicitudVoto.find( v => v.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId );
      let usuario: Usuario = this.listaMiembros.find( m => m.usuarioId == p.usuarioId ) 

      let solicitudVoto: SesionSolicitudVoto = {
        sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
        sesionParticipanteId: p.sesionParticipanteId,
        sesionSolicitudVotoId: votacion ? votacion.sesionSolicitudVotoId : 0,
        nombreParticipante: `${ usuario.nombres } ${ usuario.apellidos }`,
        esAprobado: votacion ? votacion.esAprobado : false,
        observacion: votacion ? votacion.observacion : null,

        sesionComiteSolicitud: elemento,


      }

      elemento.sesionSolicitudVoto.push( solicitudVoto )
    })

    this.dialog.open(VotacionSolicitudComponent, {
      width: '70em', data: elemento
    });
  }

  openDialogValidacionSolicitudesMultiple() {
    this.dialog.open(VotacionSolicitudMultipleComponent, {
      width: '70em'
    });
  }

  ngOnInit(): void {

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

  cargarRegistro(){
    this.dataSource = new MatTableDataSource( this.ObjetoComiteTecnico.sesionComiteSolicitud );    
  }

}
