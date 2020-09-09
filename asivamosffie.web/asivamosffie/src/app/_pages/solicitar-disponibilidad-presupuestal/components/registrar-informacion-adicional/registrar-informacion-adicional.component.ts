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

  objetoDisponibilidad: DisponibilidadPresupuestal;
  listaProyectos: Proyecto[] = [];

  idContratacion = 9;

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

  ) { }

  ngOnInit(): void {



    this.activatedroute.params.subscribe((params: Params) => {
      const id = params.id;

      this.budgetAvailabilityService.getDisponibilidadPresupuestalById(id)
        .subscribe(response => {
          this.objetoDisponibilidad = response;

          this.addressForm.get('objeto').setValue( this.objetoDisponibilidad.objeto );
          //this.addressForm.get('plazoMeses').setValue( this.objetoDisponibilidad.plazoMeses );
          //this.addressForm.get('plazoDias').setValue( this.objetoDisponibilidad.plazoDias );

          //this.projectContractingService.getContratacionByContratacionId( this.objetoDisponibilidad.contratacionId )
          //  .subscribe( contratacion => {
            console.log( response );
              
              this.objetoDisponibilidad.disponibilidadPresupuestalProyecto.forEach( dp => {
                this.projectService.getProjectById( dp.proyectoId )
                .subscribe( proyecto => {
                  dp.proyecto = proyecto;
                  console.log( proyecto );

                  this.listaProyectos.push( proyecto );

                })
              });
            //})
        })
    });
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

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    this.objetoDisponibilidad.objeto = this.addressForm.get('objeto').value;
    this.objetoDisponibilidad.plazoMeses = this.addressForm.get('plazoMeses').value;
    this.objetoDisponibilidad.plazoDias = this.addressForm.get('plazoDias').value;

    this.budgetAvailabilityService.createEditarDP(this.objetoDisponibilidad)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code == "200")
          this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudTradicional']);
      })

    console.log(this.objetoDisponibilidad);
  }

}
