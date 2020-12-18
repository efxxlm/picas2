import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormBuilder, Validators, FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { DisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ProjectService, Proyecto } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-registrar-informacion-adicional',
  templateUrl: './registrar-informacion-adicional.component.html',
  styleUrls: ['./registrar-informacion-adicional.component.scss']
})
export class RegistrarInformacionAdicionalComponent implements OnInit {

  objetoDisponibilidad: DisponibilidadPresupuestal = {};
  listaProyectos: Proyecto[] = [];

  addressForm = this.fb.group({
    plazoMeses: [null, Validators.required],
    plazoDias: [null, Validators.required],
    objeto: [null, Validators.required],
  });

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
    private activatedroute: ActivatedRoute,
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router,
    private projectContractingService: ProjectContractingService,
    private projectService: ProjectService,

  ) {
    this.activatedroute.params.subscribe((params: Params) => {
      console.log(params);
      this.objetoDisponibilidad.contratacionId = params.idContratacion;
      this.objetoDisponibilidad.disponibilidadPresupuestalId = params.idDisponibilidadPresupuestal;
      this.objetoDisponibilidad.tipoSolicitudCodigo=params.idTipoSolicitud;
      if (this.objetoDisponibilidad.disponibilidadPresupuestalId > 0) {
        this.cargarDisponibilidadPre();

      } else {
        this.cargarDisponibilidadNueva();
      }

    });
  }

  cargarDisponibilidadPre() {

    this.budgetAvailabilityService.getDisponibilidadPresupuestalById(this.objetoDisponibilidad.disponibilidadPresupuestalId)
      .subscribe(response => {
        this.objetoDisponibilidad = response;
        this.addressForm.get('objeto').setValue(this.objetoDisponibilidad.objeto);
        this.addressForm.get('plazoMeses').setValue(this.objetoDisponibilidad.plazoMeses);
        this.addressForm.get('plazoDias').setValue(this.objetoDisponibilidad.plazoDias);

        this.projectContractingService.getContratacionByContratacionId( this.objetoDisponibilidad.contratacionId )
        .subscribe(
          contratacion => {
          contratacion.contratacionProyecto.forEach(cp => {
            cp.proyecto.contratacionProyectoAportante=cp.contratacionProyectoAportante;
            this.listaProyectos.push(cp.proyecto);
          });
        });
      })

  }

  cargarDisponibilidadNueva() {

    this.objetoDisponibilidad.disponibilidadPresupuestalProyecto = [];

    this.budgetAvailabilityService.getReuestCommittee()
      .subscribe( 
        listaSolicitudes => {
        listaSolicitudes.forEach( solicitud => {
          if ( solicitud.contratacionId == this.objetoDisponibilidad.contratacionId ){
            this.objetoDisponibilidad.fechaSolicitud = solicitud.fechaSolicitud;
            this.objetoDisponibilidad.numeroSolicitud = solicitud.numeroSolicitud;
            this.objetoDisponibilidad.opcionContratarCodigo = solicitud.opcionContratar;
            this.objetoDisponibilidad.valorSolicitud = solicitud.valorSolicitud;
            this.objetoDisponibilidad.tipoSolicitudCodigo = solicitud.tipoSolicitudCodigo? solicitud.tipoSolicitudCodigo:this.objetoDisponibilidad.tipoSolicitudCodigo;            
          }
          
        }),
        err => {
          console.log( err );
        }
      })

    this.projectContractingService.getContratacionByContratacionId( this.objetoDisponibilidad.contratacionId )
      .subscribe(
        contratacion => {
          this.objetoDisponibilidad.fechaComiteTecnicoNotMapped=contratacion.fechaComiteTecnicoNotMapped;
        contratacion.contratacionProyecto.forEach(cp => {
          cp.proyecto.contratacionProyectoAportante=cp.contratacionProyectoAportante;
          
          this.listaProyectos.push(cp.proyecto);
          /*this.projectService.getProjectById(cp.proyectoId)
            .subscribe(proyecto => {
              let aporntantes=cp.contratacionProyectoAportante;
              cp.proyecto = proyecto;
              cp.proyecto.apo
              console.log(proyecto);
        
              
        
            })*/
        });
        console.log(this.listaProyectos);
        },
        err => {
          console.log( err );
        }
      );

  }

  ngOnInit(): void {
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  validatenomore30()
  {
    if(this.addressForm.value.plazoDias>30)
    {
      this.openDialog("","<b>El valor ingresado en dias no puede ser superior a 30</b>");
      this.addressForm.get("plazoDias").setValue("");
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

  onSubmit() {

    if ( this.addressForm.valid ){

    this.objetoDisponibilidad.objeto = this.addressForm.get('objeto').value;
    this.objetoDisponibilidad.plazoMeses = this.addressForm.get('plazoMeses').value;
    this.objetoDisponibilidad.plazoDias = this.addressForm.get('plazoDias').value;
      console.log(this.objetoDisponibilidad);
    this.budgetAvailabilityService.createOrEditInfoAdditional(this.objetoDisponibilidad)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if (respuesta.code == "200")
          this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudTradicional']);
      })

    }
    else{
      this.openDialog('','<b>Por favor ingrese todos los campos obligatorios.</b>')
    }
    
    console.log(this.objetoDisponibilidad);
  }

}
