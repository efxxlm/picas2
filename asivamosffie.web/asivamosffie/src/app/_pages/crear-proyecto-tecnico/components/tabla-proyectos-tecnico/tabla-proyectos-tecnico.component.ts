import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource,MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';


export interface RegistrosCargados {
  id:number;
  fecha:string,
  departamento:string,
    municipio:string,
    institucion:string,
    sede:string,
    estado:string,
    estadoj:string    ,
    gestion:string
}

@Component({
  selector: 'app-tabla-proyectos-tecnico',
  templateUrl: './tabla-proyectos-tecnico.component.html',
  styleUrls: ['./tabla-proyectos-tecnico.component.scss']
})
export class TablaProyectosTecnicoComponent {
  displayedColumns: string[] = ['fecha','departamento','municipio','institucion','sede','estado','estadoj','gestion'];
  

  columnas = [
    { titulo: 'Fecha',name: 'fecha' },
    { titulo: 'Departamento',name: 'departamento' },
    { titulo: 'Municipio',name: 'municipio' },
    { titulo: 'Institución Educativa',name: 'institucion' },
    { titulo: 'Sede',name: 'sede' },
    { titulo: 'Estado del registro',name: 'estado' },
    { titulo: 'Estado jurídico de los predios',name: 'estadoj' },
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataSource= new MatTableDataSource();
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
      width: '28em',
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
        this.projectService.deleteProjectById(this.proyectoid).subscribe(respuesta => {
          let proyecto = respuesta;
          if(respuesta)
          {
            this.inicializar();
            this.openDialog('', "<b>La información a sido eliminada correctamente.</b>");
          }
          else
          {
            this.openDialog('', "<b>El registro tiene información que depende de él no se puede eliminar.</b>");
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

  inicializar()
  {
    this.projectService.getListProjects().subscribe(respuesta => {
      let datos:RegistrosCargados[]=[];
      console.log(respuesta);
      respuesta.forEach(element => {
        datos.push({fecha:element.fecha
          ,id:element.proyectoId,departamento:element.departamento,municipio:element.municipio,
          estado:element.estadoRegistro,estadoj:element.estadoJuridicoPredios,
          institucion:element.institucionEducativa,sede:element.sede,gestion:element.proyectoId});
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
    this.router.navigate(['/crearProyecto/crearProyecto', { id: gestion.id}]);
  }

  eliminar(gestion:any)
  {
    console.log(gestion);  
    this.proyectoid=gestion.id;
    this.openDialogSiNo('', "¿Está seguro de eliminar este registro?",);  
    
  }
  
  enviar(gestion:any)
  {
    console.log(gestion);
  }
}
