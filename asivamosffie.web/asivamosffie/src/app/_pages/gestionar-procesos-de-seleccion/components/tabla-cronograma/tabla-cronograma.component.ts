import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ProcesoSeleccionService, ProcesoSeleccionCronograma, ProcesoSeleccionMonitoreo, ProcesoSeleccionCronogramaMonitoreo, EstadosProcesoSeleccion, EstadosProcesoSeleccionMonitoreo } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, from } from 'rxjs';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { CommonService, Dominio, Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EstadosSolicitud, EstadosSolicitudCronograma } from 'src/app/_interfaces/project-contracting';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
export interface ProcesosElement {
  id: any;
  tipo: string;
  numero: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  estadoDelSolicitud: string;
}
@Component({
  selector: 'app-tabla-cronograma',
  templateUrl: './tabla-cronograma.component.html',
  styleUrls: ['./tabla-cronograma.component.scss']
})
export class TablaCronogramaComponent implements OnInit {

  displayedColumns: string[] = [ 'tipo', 'numero', 'fechaSolicitud', 'numeroSolicitud', 'estadoDelSolicitud', 'id'];
  dataSource = new MatTableDataSource();
  

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @Input() editMode: any = {};

  
  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  estadosProcesoSeleccionMonitoreo = EstadosProcesoSeleccionMonitoreo;

  bitEditar=true;

  addressForm = this.fb.array([]);
  maxDate: Date;
  listaCronograma: ProcesoSeleccionCronograma[] = [];
  idProcesoSeleccion: number = 0;
  listaetapaActualProceso: Dominio[]=[];

  editorStyle = {
    height: '100px',
    width: '600px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };



  constructor(
    private fb: FormBuilder,
    private procesoSeleccionService: ProcesoSeleccionService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    private commonService:CommonService

  ) {
    this.maxDate = new Date();
  }


  ngOnInit(): void {

    this.commonService.listaEtapaActualProceso().subscribe(result=>{
      this.listaetapaActualProceso=result; 
    });
    this.activatedRoute.params.subscribe(parametro => {
      this.idProcesoSeleccion = parametro['id'];

      this.procesoSeleccionService.listaProcesoSeleccionCronogramaMonitoreo(this.idProcesoSeleccion).subscribe(monitoreo => {
        if(monitoreo.length==0)
        {
          this.procesoSeleccionService.listaActividadesByIdProcesoSeleccion(this.idProcesoSeleccion).subscribe(lista => {

            let listaActividades = this.addressForm as FormArray;
            this.listaCronograma = lista;            
    
            lista.forEach(cronograma => {
              let grupo = this.crearActividad();
              const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
              grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
              grupo.get('descripcion').setValue(cronograma.descripcion);
              grupo.get('fecha').setValue(cronograma.fechaMaxima);
              grupo.get('etapaActualProceso').setValue(etapaActualproceso),
              listaActividades.push(grupo);
    
            })
    
          })
        }
        else{
          let listaActividades = this.addressForm as FormArray;
          this.listaCronograma = monitoreo[monitoreo.length-1].procesoSeleccionCronogramaMonitoreo;                
          if(this.listaCronograma)
          {
            
            this.listaCronograma.forEach(cronograma => {
              let grupo = this.crearActividad();
              const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
              grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
              grupo.get('descripcion').setValue(cronograma.descripcion);
              grupo.get('fecha').setValue(cronograma.fechaMaxima);
              grupo.get('etapaActualProceso').setValue(etapaActualproceso),    
              listaActividades.push(grupo);    
              if(cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.Creada && cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteFiduciario &&
                cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteTecnico)
                {
                  this.bitEditar=false;
                }
            })
          }            
        }
      });
      let listaProcesos: ProcesosElement[] = []; 

      forkJoin([

        this.procesoSeleccionService.listaProcesoSeleccionCronogramaMonitoreo( this.idProcesoSeleccion ),
        this.commonService.listaTipoProcesoSeleccion(),
        this.commonService.listaEstadoProcesoSeleccionMonitoreo(),
        this.commonService.listaEtapaProcesoSeleccion(),

      ]).subscribe( respuesta => {

          respuesta[0].forEach(proceso => {
            console.log("proceso");
            console.log(proceso);
            let nombreTipo = respuesta[1].find( p => p.codigo == proceso.procesoSeleccion.tipoProcesoCodigo )
            let nombreEstado = respuesta[2].find( p => p.codigo == proceso.estadoActividadCodigo )
            let nombreEtapa = respuesta[3].find( p => p.codigo == proceso.estadoActividadCodigo )
            
            /*if (nombreTipo)   proceso.procesoSeleccion.tipoProcesoNombre = nombreTipo.nombre;
            if (nombreEstado) proceso.procesoSeleccion.estadoProcesoSeleccionNombre = nombreEstado.nombre;
            if (nombreEtapa)  proceso.procesoSeleccion.etapaProcesoSeleccionNombre = nombreTipo.nombre;
*/
            listaProcesos.push( {estadoDelSolicitud:nombreEstado.nombre,
              fechaSolicitud:proceso.fechaCreacion,
              id:{estadoActividadCodigo:proceso.estadoActividadCodigo,
                numeroProceso:proceso.numeroProceso,
                procesoSeleccionCronogramaMonitoreo:proceso.procesoSeleccionCronogramaMonitoreo,
                procesoSeleccionId:proceso.procesoSeleccionId, 
                procesoSeleccionMonitoreoId:proceso.procesoSeleccionMonitoreoId,
                fechaCreacion:proceso.fechaCreacion,
                usuarioCreacion:proceso.usuarioCreacion,
                eliminado:proceso.eliminado,
                enviadoComiteTecnico:proceso.enviadoComiteTecnico
              },
              numero:proceso.procesoSeleccion.numeroProceso,
              numeroSolicitud:proceso.numeroProceso,tipo:nombreTipo.nombre} );
          });
          
          this.dataSource = new MatTableDataSource( listaProcesos );

          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
          this.paginator._intl.nextPageLabel = 'Siguiente';
          this.paginator._intl.previousPageLabel = 'Anterior';

      })
    })
  }

  agregaFuente() {
    this.addressForm.push(this.crearActividad());
  }

  crearActividad(): FormGroup {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(500)
      ])],
      fecha: [null, Validators.required],
      etapaActualProceso: [null, Validators.required],
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {

    let listaActividades = this.addressForm as FormArray;
    let listaCronograma:ProcesoSeleccionMonitoreo={estadoActividadCodigo:null,
      numeroProceso:null,
      procesoSeleccionCronogramaMonitoreo:[],
      procesoSeleccionId:null, procesoSeleccionMonitoreoId:null
    };

    let i = 0;
    listaActividades.controls.forEach(control => {
      let procesoSeleccionCronograma: ProcesoSeleccionCronogramaMonitoreo = {
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,
        descripcion: control.get('descripcion').value,
        fechaMaxima: control.get('fecha').value,
        etapaActualProcesoCodigo: control.get('etapaActualProceso').value?control.get('etapaActualProceso').value.codigo:null,
        //procesoSeleccionId: this.idProcesoSeleccion,
        numeroActividad: i,

      }
      listaCronograma.procesoSeleccionCronogramaMonitoreo.push(procesoSeleccionCronograma);
      listaCronograma.procesoSeleccionId=this.idProcesoSeleccion;
      i++
    })
    this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo(listaCronograma).subscribe(respuesta => {
     
      if (respuesta.code == "200")
        this.openDialog("", respuesta.message,true);
    });

    /*from(this.listaCronograma)
      .pipe(mergeMap(cronograma => this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo(cronograma)
        .pipe(
          tap()
        )
      ),
        toArray())
      .subscribe(respuesta => {
        let res = respuesta[0] as Respuesta
        if (res.code == "200")
          this.openDialog("", res.message);
        console.log(respuesta);

        //jflorez deshabilito el modo visualización
        //this.editMode.valor = false; 
      })
    */
  }

  openDialog(modalTitle: string, modalText: string,refrescar:boolean=false) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    if(refrescar)
    {
      dialogRef.afterClosed().subscribe(result => {
        location.reload();
       }); 
    }
  }

  
  onDetalle(id:ProcesoSeleccionMonitoreo,tipo:number){
    //this.editMode.valor = !this.editMode.valor;
    console.log("editar");
    console.log(id);
    let listaActividades = this.addressForm as FormArray;
    listaActividades.clear();
    id.procesoSeleccionCronogramaMonitoreo.forEach(cronograma => {
      let grupo = this.crearActividad();
      const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
      grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
      grupo.get('descripcion').setValue(cronograma.descripcion);
      grupo.get('fecha').setValue(cronograma.fechaMaxima);
      grupo.get('etapaActualProceso').setValue(etapaActualproceso),    
      
      listaActividades.push(grupo);
      if(cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.Creada && cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteFiduciario &&
        cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteTecnico)
        {
          this.bitEditar=false;
        }
    })
    window.scroll(0,0);
    //location.reload();
  }

  onEnviarSolicitud(id:any){
    id.enviadoComiteTecnico=true;
    this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo( id ).subscribe( respuesta => {
      this.openDialog("", respuesta.message);
      if ( respuesta.code == "200" )
        this.ngOnInit();
    })
  }

  onEliminar(id:any){
    this.openDialogSiNo('','¿Está seguro de eliminar este registro?',id)
  }

  eliminarRegistro(id:ProcesoSeleccionCronogramaMonitoreo ){    
    this.procesoSeleccionService.deleteProcesoSeleccionCronogramaMonitoreo( id.procesoSeleccionMonitoreoId ).subscribe( respuesta => {
      let r = respuesta as Respuesta;
       if ( r.code == "200" )
       {
         this.openDialog("", "<b>La información se ha eliminado correctamente.</b>",true);
         //this.router.navigate(['/seleccion']);

       }else
        this.openDialog("", r.message);
    })
  }

  openDialogSiNo(modalTitle: string, modalText: string , id:any) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.eliminarRegistro(id);
      }           
    });
  }


}
