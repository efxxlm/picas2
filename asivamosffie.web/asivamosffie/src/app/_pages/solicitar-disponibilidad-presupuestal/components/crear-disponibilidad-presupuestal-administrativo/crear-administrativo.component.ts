import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { ProyectoAdministrativo, ProyectoAdministrativoAportante } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestal, DisponibilidadPresupuestalProyecto, ListAdminProyect, ListConcecutivoProyectoAdministrativo } from 'src/app/_interfaces/budgetAvailability';

@Component({
  selector: 'app-crear-administrativo',
  templateUrl: './crear-administrativo.component.html',
  styleUrls: ['./crear-administrativo.component.scss']
})
export class CrearDisponibilidadPresupuestalAdministrativoComponent implements OnInit {

  formulario = this.fb.group({
    disponibilidadPresupuestalId: [],
    proyectoAdministrativoId: [],

    objeto: [null, Validators.required],
    consecutivo: [null, Validators.required],

  })

  listaProyectos: ListConcecutivoProyectoAdministrativo[] = []
  listaAportantes: ListAdminProyect = {}
  objetoDispinibilidad: DisponibilidadPresupuestal = {}

  editorStyle = {
    height: '45px'
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
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router,
    private activatedRoute: ActivatedRoute,

  ) 
  {
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      forkJoin([
        this.budgetAvailabilityService.getListCocecutivoProyecto(),        
      ]).subscribe( respuesta => {
          this.listaProyectos = respuesta[0];
          if(parametros.id>0)
          {
            this.budgetAvailabilityService.getDisponibilidadPresupuestalById( parametros.id ).subscribe(
              result=>{
                this.objetoDispinibilidad = result;
  
                //console.log( this.objetoDispinibilidad );
      
                let proyecto = this.objetoDispinibilidad.disponibilidadPresupuestalProyecto[0];
      
                let proyectoSeleccionado = this.listaProyectos.find( p => p.proyectoId == proyecto.proyectoAdministrativoId );
      
                this.formulario.get('consecutivo').setValue( proyectoSeleccionado )
                this.formulario.get('objeto').setValue( this.objetoDispinibilidad.objeto )
                this.formulario.get('proyectoAdministrativoId').setValue( proyecto.proyectoAdministrativoId )
                this.formulario.get('disponibilidadPresupuestalId').setValue( this.objetoDispinibilidad.disponibilidadPresupuestalId )
      
                this.changeProyecto();              
              }
            )            
          }          
        })
    })

    
  }

  changeProyecto(){

    let proyecto = this.formulario.get('consecutivo').value;
    console.log( proyecto )
    this.listaAportantes = proyecto;
    /*this.budgetAvailabilityService.getAportantesByProyectoAdminId( proyecto.proyectoId )
      .subscribe( lista  => {
        
      })*/
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
  noGuardado=true;
  ngOnDestroy(): void {
    if ( this.noGuardado===true && this.formulario.dirty) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.enviarObjeto();          
        }           
      });
    }
  };

  enviarObjeto() {

    let aportante = this.listaAportantes[0];

    let valor=0;
    this.listaAportantes.aportanteFuenteFinanciacion.forEach(element => {
      valor+=element.valorFuente;
    });
    let disponibilidad: DisponibilidadPresupuestal = {
      disponibilidadPresupuestalId: this.formulario.get('disponibilidadPresupuestalId').value,
      objeto: this.formulario.get('objeto').value,
      tipoSolicitudCodigo: '3',
      valorSolicitud: valor,
     
      disponibilidadPresupuestalProyecto: []
      
    }

    let proyectoSeleccionado = this.formulario.get('consecutivo').value
    let iddiproyectoid=this.objetoDispinibilidad.disponibilidadPresupuestalProyecto?this.objetoDispinibilidad.disponibilidadPresupuestalProyecto[0].disponibilidadPresupuestalProyectoId:0;
    let proyecto: DisponibilidadPresupuestalProyecto = {
       disponibilidadPresupuestalProyectoId: iddiproyectoid,
      proyectoAdministrativoId: proyectoSeleccionado ? proyectoSeleccionado.proyectoId : null,
    }

    disponibilidad.disponibilidadPresupuestalProyecto.push( proyecto );

    this.budgetAvailabilityService.createOrEditProyectoAdministrtivo( disponibilidad )
      .subscribe( respuesta => {
        this.openDialog( '', `<b>${respuesta.message}</b>` )
        if ( respuesta.code == "200" )
        {
          this.router.navigate(['/solicitarDisponibilidadPresupuestal'])
          this.noGuardado=false;
        }
          
      })

     console.log( disponibilidad, this.formulario.get('consecutivo').value.proyectoId );
    
  }

}
