import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';


@Component({
  selector: 'app-table-solicitud-contratacion',
  templateUrl: './table-solicitud-contratacion.component.html',
  styleUrls: ['./table-solicitud-contratacion.component.scss']
})
export class TableSolicitudContratacionComponent implements OnInit {

  estadosSolicitud = EstadosSolicitud;

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'opcionPorContratar',
    'estadoSolicitud',
    'estadoDelIngreso',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
              private projectContractingService: ProjectContractingService,
              public dialog: MatDialog
  ) 
  
  { }

  ngOnInit(): void {

    this.projectContractingService.getListContratacion().subscribe( response => {

      this.dataSource = new MatTableDataSource( response );
      console.log( response );
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    })
    
    
  }

  detallarSolicitud(id: number) {
    console.log(id);
  }

  onDelete( id: number ){
    this.openDialogSiNo('','“¿Está seguro de eliminar este registro?', id)  
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe( result => {

      if(result)
      {
        this.eliminarSolicitud( e );
      }           
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  eliminarSolicitud(id: number) {
    console.log(id);
    this.projectContractingService.eliminarContratacion( id )
      .subscribe( respuesta => {
          
        this.openDialog('Solicitud Contratacion', 'La información se ha eliminado correctamente')
        this.ngOnInit();          

      });
  }

  enviarSolicitud( id: number){
    this.projectContractingService.changeStateContratacionByIdContratacion( id, this.estadosSolicitud.RechazadaPorComiteTecnico )
      .subscribe( respuesta => {
        this.openDialog('Solicitud Contratacion', respuesta.message )
        this.ngOnInit();          

      });
  }

}
