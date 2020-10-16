import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource,MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface RegistrosCargados {
  id: number;    
  consecutivo: number;
  estado:string;
  enviado:boolean;
}


@Component({
  selector: 'app-tabla-proyectos-admin',
  templateUrl: './tabla-proyectos-admin.component.html',
  styleUrls: ['./tabla-proyectos-admin.component.scss']
})
export class TablaProyectosAdminComponent {
  displayedColumns: string[] = ['consecutivo', 'estado','gestion'];
  dataSource = new MatTableDataSource([]);

  columnas = [
    { titulo: 'Consecutivo proyecto administrativo', name: 'consecutivo' },
    { titulo: 'Estado del registro', name: 'estado' }
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  proyectoid: number;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private projectService: ProjectService,public dialog: MatDialog,
    public datepipe: DatePipe,private router: Router,) {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '38em',
      data: { modalTitle, modalText }
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.projectService.DeleteProyectoAdministrativoByProyectoId(this.proyectoid).subscribe(respuesta => {
          let proyecto = respuesta;
          if(respuesta)
          {
            this.inicializar();
            this.openDialog('', "Proyecto eliminado correctamente");
          }
          else
          {
            this.openDialog('', "Hubo un error al eliminar el proyecto, por favor intenta nuevamente.");
          }
        },
          err => {
            let mensaje: string;
            console.log(err);
            if (err.message) {
              mensaje = err.message;
            }
            else if (err.error.message) {
              mensaje = err.error.message;
            }
            this.openDialog('Error', mensaje);
          },
          () => {
            // console.log('terminó');
          });    
      }           
    });
  }
  inicializar() {
    this.projectService.ListAdministrativeProject().subscribe(respuesta => {
      let datos:RegistrosCargados[]=[];
      console.log(respuesta);
      respuesta.forEach(element => {
        datos.push({id:element.proyectoAdminitracionId,estado:element.estado?"Completo":"Incompleto",consecutivo:element.proyectoAdminitracionId,enviado:element.enviado});
      });
      this.dataSource=new MatTableDataSource<RegistrosCargados>(datos);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
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
  
  ngOnInit(): void {
    this.inicializar();
  }

  ver(gestion:any)
  {
    console.log(gestion); 
    this.router.navigate(['/crearProyectoAdministrativo/crearProyecto', { id: gestion.id}]);   
  }

  eliminar(gestion:any)
  {
    this.proyectoid=gestion.id;
    this.openDialogSiNo('', "¿Está seguro de eliminar este registro?",);   
  }
  
  enviar(gestion:any)
  {
    this.proyectoid=gestion.id;
    this.projectService.EnviarProyectoAdministrativoByProyectoId(this.proyectoid).subscribe(respuesta => {
      let proyecto = respuesta;
      {
        this.inicializar();
        this.openDialog('Su solicitud del proyecto '+gestion.id, "Ha sido enviada para que inicie el tramite <br><b>“Solicitud de documento de disponibilidad presupuestal”.</b>",);
        
      }
    });
    
  }

}
