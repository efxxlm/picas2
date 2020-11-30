import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';

export interface RegistrosCargados {
  id: number;
  fechaCargue: string;
  totalRegistros: number;
  registrosValidos: number;
  registrosInvalidos: number;
  gestion:string;
}


@Component({
  selector: 'app-tabla-proyectos',
  templateUrl: './tabla-proyectos.component.html',
  styleUrls: ['./tabla-proyectos.component.scss']
})

export class TablaProyectosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCargue', 'totalRegistros', 'registrosValidos', 'registrosInvalidos','gestion'];
  dataSource = new MatTableDataSource();

  columnas = [
    { titulo: 'Fecha de cargue', name: 'fechaCargue' },
    { titulo: 'Número total de registros ', name: 'totalRegistros' },
    { titulo: 'Número de registros validos ', name: 'registrosValidos' },
    { titulo: 'Número de registros inválidos ', name: 'registrosInvalidos' },
    
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private projectService: ProjectService,public dialog: MatDialog,public datepipe: DatePipe) {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  ngOnInit(): void {
    this.projectService.getListProjectsFileProject().subscribe(respuesta => {
      let datos:RegistrosCargados[]=[];
      console.log(respuesta);
      respuesta.forEach(element => {
        datos.push({
          fechaCargue:this.datepipe.transform(element.fechaCreacion, 'yyyy-MM-dd'),
          id:element.archivoCargueId,
          registrosInvalidos:element.cantidadRegistrosInvalidos,
          registrosValidos:element.cantidadRegistrosValidos,
          totalRegistros:element.cantidadRegistros,
          gestion:element.nombre
        });
      });
      this.dataSource=new MatTableDataSource<RegistrosCargados>(datos);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.getRangeLabel = function (page, pageSize, length) {        
        return (page+1).toString()+" de "+length.toString();
      };
      this.paginator._intl.previousPageLabel = 'Anterior';
    }, 
    err => {
      let mensaje: string;
      console.log(err);
      if (err.message){
        mensaje = err.message;
      }
      else if (err.error.message){
        mensaje = err.error.message;
      }
      this.openDialog('Error', mensaje);
   },
   () => {
    //console.log('terminó');
   });
    
  }

  ver(gestion:any)
  {
    console.log(gestion);
    this.projectService.getFileByName(gestion.gestion).subscribe(respuesta => {
      let documento="documento.xlsx";
      //console.log(documento);
              
        //console.log(result);
        /*var url = window.URL.createObjectURL(result);
        window.open(url);
        //console.log("download result ", result);*/
        var text = documento,
        blob = new Blob([respuesta], { type: 'application/octet-stream' }),
        anchor = document.createElement('a');
        anchor.download = documento;
        //anchor.href = (window.webkitURL || window.URL).createObjectURL(blob);
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        //console.log(anchor);
        anchor.click();
    }, 
    err => {
     
      this.openDialog('Error', "No se encontró el documento.");
   },
   () => {
    //console.log('terminó');
   });
    
  }

}
