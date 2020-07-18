import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';


@Component({
  selector: 'app-btn-registrar',
  templateUrl: './btn-registrar.component.html',
  styleUrls: ['./btn-registrar.component.scss']
})
export class BtnRegistrarComponent implements OnInit {

  verAyuda = false;

  tiposAportante: Dominio[] = [];

  regitrarAporteForm = this.fb.group({
    tipoAportante: [null, Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private commonService: CommonService) { }

  ngOnInit(): void {
    this.commonService.listaTipoAportante().subscribe( tip => {
      this.tiposAportante = tip; 
    })
    
  }

  onSubmit() {
    if (this.regitrarAporteForm.valid) {
      let idTipoAportante: number;
      idTipoAportante = this.regitrarAporteForm.get('tipoAportante').value.dominioId
      this.router.navigate(['./registrarFuentes', idTipoAportante,0]);
    }
  }

}
